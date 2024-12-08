using System.Globalization;
using BytecodeGenerationSimplifier;
using CommonBytecode.Data.Structures;
using CommonBytecode.Enums;
using ExceptionsManager;
using QuarkCFrontend;
using SharpLibrariesImporter;

namespace AsgToBytecodeTranslator;

public class AsgToBytecodeTranslator
{
    private readonly List<BytecodeFunction> _functions = [];
    private readonly ImportsManager _importsManager = new();

    private BytecodeFunction CurFunction => _functions[^1];
    private List<BytecodeInstruction> CurBytecode => CurFunction.Code.Instructions;

    public BytecodeModule Translate(AsgNode root)
    {
        Visit(root);
        return new BytecodeModule("Program", _functions);
    }

    // split to classes
    public void Visit(AsgNode node)
    {
        switch (node.NodeType)
        {
            case AsgNodeType.Unknown:
                break;
            case AsgNodeType.Import:
                _importsManager.Import(GetString(node.Children[0].Text));
                break;
            case AsgNodeType.String:
                var str = GetString(node.Text);
                CurBytecode.Add(new BytecodeInstruction(InstructionType.PushConst, [str]));
                // ...
                break;
            case AsgNodeType.FunctionCreating:
                _functions.Add(new BytecodeFunction(node.Text, new Bytecode([])));
                Visit(node.Children[^1]);
                break;
            case AsgNodeType.SetOperation:
                break;
            case AsgNodeType.Number:
                var number = double.Parse(node.Text, CultureInfo.InvariantCulture);
                CurBytecode.Add(new BytecodeInstruction(InstructionType.PushConst, [number]));
                break;
            case AsgNodeType.Type:
                break;
            case AsgNodeType.FunctionCall:
                Visit(node.Children);
                
                var functionName = node.Text;
                
                if (_importsManager.Have(functionName))
                {
                    var @delegate = _importsManager.GetDelegateByName(functionName);
                    var instructions = (Span<BytecodeInstruction>) [..SimpleBytecodeGenerator.CallSharp(@delegate)];
                    CurBytecode.AddRange(instructions);
                }
                else
                {
                    CurBytecode.Add(new BytecodeInstruction(InstructionType.CallFunc, [node.Text]));
                }

                break;
            case AsgNodeType.If:
                break;
            case AsgNodeType.Else:
                break;
            case AsgNodeType.ElseIf:
                break;
            case AsgNodeType.Return:
                Visit(node.Children[0]);
                CurBytecode.Add(new BytecodeInstruction(InstructionType.Ret, []));
                break;
            case AsgNodeType.ForLoop:
                break;
            case AsgNodeType.WhileLoop:
                break;
            case AsgNodeType.Multiplication:
                break;
            case AsgNodeType.Division:
                break;
            case AsgNodeType.Addition:
                break;
            case AsgNodeType.Subtraction:
                break;
            case AsgNodeType.LessThan:
                break;
            case AsgNodeType.GreaterThan:
                break;
            case AsgNodeType.LessThanOrEqual:
                break;
            case AsgNodeType.GreaterThanOrEqual:
                break;
            case AsgNodeType.Equal:
                break;
            case AsgNodeType.NotEqual:
                break;
            case AsgNodeType.Modulus:
                break;
            case AsgNodeType.Scope:
                Visit(node.Children);
                break;
            default:
                Throw.InvalidOpEx();
                break;
        }
    }

    private static string GetString(string text) => text[1..^1];

    private void Visit(List<AsgNode> nodeChildren)
    {
        foreach (var node in nodeChildren)
            Visit(node);
    }
}