using AsgToBytecodeTranslator;
using CommonBytecode.Data.Structures;
using CommonBytecode.Enums;
using CommonFrontendApi;
using DefaultAstImpl.Asg;
using DefaultLexerImpl;
using DictionaryExtensions;
using QuarkExtension;
using SharpAnyType;
using static BytecodeGenerationSimplifier.SimpleBytecodeGenerator;

namespace QuarkListInitializer;

public class QuarkListInitializer : IQuarkExtension
{
    public LexerConfiguration<QuarkLexemeType> ExtendLexerConfiguration(LexerConfiguration<QuarkLexemeType> current) =>
        current;

    public AsgBuilderConfiguration<QuarkLexemeType> ExtendAsgBuilderConfiguration(
        AsgBuilderConfiguration<QuarkLexemeType> current
    )
    {
        current.CreatorLevels.CreateNewOrAdd(2.5f, new ListInitializerNodeCreator());
        return current;
    }

    public Func<AsgToBytecodeData<QuarkLexemeType>, bool> GetAsgNodeHandler() => ListInitializerCreator;

    private bool ListInitializerCreator(AsgToBytecodeData<QuarkLexemeType> data)
    {
        if (data.Node.NodeType != ListInitializerNodeCreator.ListInitializer) return false;

        var listName = Guid.NewGuid().ToString();
        data.CurBytecode.Add(CallSharp(CreateList));
        data.CurBytecode.Add(new BytecodeInstruction(InstructionType.MakeVariables,
            [new Any(new BytecodeVariable(listName, AnyValueType.SomeSharpObject), AnyValueType.SomeSharpObject)]));
        data.CurBytecode.Add(new BytecodeInstruction(InstructionType.SetLocal, [listName]));
        foreach (var nodeChild in data.Node.Children)
        {
            data.CurBytecode.Add(new BytecodeInstruction(InstructionType.LoadLocal, [listName]));
            data.AsgToBytecodeTranslator.Visit(nodeChild);
            data.CurBytecode.Add(CallSharp(AddToList));
            data.CurBytecode.Add(new BytecodeInstruction(InstructionType.Drop, []));
        }

        data.CurBytecode.Add(new BytecodeInstruction(InstructionType.LoadLocal, [listName]));
        return true;
    }

    public static Any CreateList() => new(new List<Any>(), AnyValueType.SomeSharpObject);

    public static Any AddToList(Any lst, Any value)
    {
        lst.Get<List<Any>>().Add(value);
        return Any.Nil;
    }
}