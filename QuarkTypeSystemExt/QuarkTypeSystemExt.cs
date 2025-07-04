using AsgToBytecodeTranslator;
using BytecodeGenerationSimplifier;
using CommonBytecode.Data.Structures;
using CommonBytecode.Enums;
using CommonExtensions;
using CommonFrontendApi;
using DefaultAstImpl.Asg;
using DefaultLexerImpl;
using ExceptionsManager;
using QuarkExtension;
using SharpAnyType;

namespace QuarkTypeSystemExt;

public class QuarkTypeSystemExt : IQuarkExtension
{
    public static readonly QuarkLexemeType Colon = QuarkLexemeTypeHelper.GetNextFreeNumber();

    private readonly Dictionary<string, string> _varToType = [];
    private bool _tryingToAddVarType;

    public LexerConfiguration<QuarkLexemeType> ExtendLexerConfiguration(LexerConfiguration<QuarkLexemeType> current)
    {
        current.Patterns.Add(new LexemePattern<QuarkLexemeType>("\\:", Colon));
        return current;
    }

    public AsgBuilderConfiguration<QuarkLexemeType> ExtendAsgBuilderConfiguration(
        AsgBuilderConfiguration<QuarkLexemeType> current
    )
    {
        current.CreatorLevels.CreateNewOrAdd(1.5f, new IdentifierTypeNodeCreator());
        return current;
    }

    public Func<AsgToBytecodeData<QuarkLexemeType>, bool> GetAsgNodeHandler() => NodesHandler;

    private bool NodesHandler(AsgToBytecodeData<QuarkLexemeType> data)
    {
        var node = data.Node;
        var translator = data.AsgToBytecodeTranslator;

        if (TryAddVarType(node, data)) return true;

        if (TrySetOp(data, node, translator)) return true;

        return false;
    }

    private bool TrySetOp(AsgToBytecodeData<QuarkLexemeType> data, AsgNode<QuarkLexemeType> node,
        IAsgToBytecodeTranslator<QuarkLexemeType> translator)
    {
        if (node.NodeType == AsgNodeType.SetOperation)
        {
            if (node.Children[0].NodeType == AsgNodeType.Identifier)
            {
                var varName = node.Children[0].Text;
                TryAddVarType(node.Children[0], data);

                translator.Visit(node.Children[^1]);

                data.CurBytecode.AddRange(SimpleBytecodeGenerator.Dup());
                data.CurBytecode.Add(new BytecodeInstruction(InstructionType.PushConst, [GetTypeOfVar(varName)]));
                data.CurBytecode.Add(SimpleBytecodeGenerator.CallSharp(CheckForTypes));

                data.CurBytecode.Add(new BytecodeInstruction(InstructionType.SetLocal, [varName]));
            }
            else
            {
                translator.Visit(node.Children[0]);
            }

            return true;
        }

        return false;
    }

    private string GetTypeOfVar(string varName) => _varToType.GetValueOrDefault(varName, nameof(AnyValueType.Any));

    private bool TryAddVarType(AsgNode<QuarkLexemeType> node, AsgToBytecodeData<QuarkLexemeType> data)
    {
        if (_tryingToAddVarType) return false;

        _tryingToAddVarType = true;
        if (node.NodeType == AsgNodeType.Identifier)
        {
            _varToType.TryAdd(node.Text, nameof(AnyValueType.Any));
            if (node.Children is [_, { NodeType: AsgNodeType.Identifier }, ..])
                _varToType[node.Text] = node.Children[1].Text;
            data.AsgToBytecodeTranslator.Visit(node);
            _tryingToAddVarType = false;
            return true;
        }

        _tryingToAddVarType = false;
        return false;
    }

    private static void CheckForTypes(Any v1, Any v2)
    {
        var t2 = (string)v2.Value;
        var t1 = v1.Type.ToString();
        if (t2 == nameof(AnyValueType.Any)) return;

        Throw.AssertAlways(t2 == t1, $"Type of variable doesn't match with value type: {t2} != {t1}");
    }
}