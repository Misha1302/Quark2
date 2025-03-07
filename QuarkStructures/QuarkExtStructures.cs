using AsgToBytecodeTranslator;
using BytecodeGenerationSimplifier;
using CommonBytecode.Data.Structures;
using CommonBytecode.Enums;
using CommonFrontendApi;
using DefaultAstImpl.Asg;
using DefaultLexerImpl;
using QuarkExtension;
using SharpAnyType;

namespace QuarkStructures;

using BI = BytecodeInstruction;

public class QuarkExtStructures : IQuarkExtension
{
    public static readonly QuarkLexemeType StructType = QuarkLexemeTypeHelper.GetNextFreeNumber();
    public static readonly QuarkLexemeType FieldAccess = QuarkLexemeTypeHelper.GetNextFreeNumber();

    public LexerConfiguration<QuarkLexemeType> ExtendLexerConfiguration(
        LexerConfiguration<QuarkLexemeType> current
    )
    {
        current.Patterns.Insert(0, new LexemePattern<QuarkLexemeType>("struct", StructType));
        current.Patterns.Insert(0, new LexemePattern<QuarkLexemeType>("->", FieldAccess));
        return current;
    }

    public AsgBuilderConfiguration<QuarkLexemeType> ExtendAsgBuilderConfiguration(
        AsgBuilderConfiguration<QuarkLexemeType> current
    )
    {
        current.CreatorLevels.Insert(0, [new FieldGetNodeCreator()]);
        current.CreatorLevels.Insert(0, [new FieldSetNodeCreator()]);
        current.CreatorLevels.Add([new StructNodeCreator()]);
        return current;
    }

    public Action<AsgToBytecodeData<QuarkLexemeType>> GetUnknownAsgCodeHandler() => UnknownAsgCodeHandle;

    private void UnknownAsgCodeHandle(AsgToBytecodeData<QuarkLexemeType> data)
    {
        if (data.Node.NodeType == StructNodeCreator.StructType)
            TranslateStructTypeAsg(data);
        if (data.Node.NodeType == FieldGetNodeCreator.FieldGet)
            TranslateFieldGetAsg(data);
        if (data.Node.NodeType == FieldSetNodeCreator.FieldSet)
            TranslateFieldSetAsg(data);
    }

    private void TranslateFieldSetAsg(AsgToBytecodeData<QuarkLexemeType> data)
    {
        data.AsgToBytecodeTranslator.Visit(data.Node.Children[0]);
        data.CurBytecode.Add(new BI(InstructionType.PushConst, [data.Node.Children[1].Text.ObjectToAny()]));
        data.CurBytecode.Add(SimpleBytecodeGenerator.CallSharp(QuarkStructuresLibrary.SetField));
        data.CurBytecode.Add(new BI(InstructionType.Drop, []));
    }

    private void TranslateFieldGetAsg(AsgToBytecodeData<QuarkLexemeType> data)
    {
        data.AsgToBytecodeTranslator.Visit(data.Node.Children[0]);
        data.CurBytecode.Add(new BI(InstructionType.PushConst, [data.Node.Children[1].Text.ObjectToAny()]));
        data.CurBytecode.Add(SimpleBytecodeGenerator.CallSharp(QuarkStructuresLibrary.GetField));
    }

    private static void TranslateStructTypeAsg(AsgToBytecodeData<QuarkLexemeType> data)
    {
        // struct Vector3(x, y, z)
        var name = data.Node.Children[0].Text;
        var scope = data.Node.Children[1];
        var fields = scope.Children
            .Where(x => x.NodeType == AsgNodeType.Identifier)
            .Select(x => x.Text)
            .ToDictionary(x => x, _ => Any.Nil);

        QuarkStructuresLibrary.Instance.AddStructure(new QuarkStructure(name, fields));
    }
}