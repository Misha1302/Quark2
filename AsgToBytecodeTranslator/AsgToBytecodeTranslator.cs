using System.Globalization;
using BytecodeGenerationSimplifier;
using CommonBytecode.Data.AnyValue;
using CommonBytecode.Data.Structures;
using CommonBytecode.Enums;
using ExceptionsManager;
using QuarkCFrontend.Asg;
using SharpLibrariesImporter;

namespace AsgToBytecodeTranslator;

public class AsgToBytecodeTranslator
{
    private readonly Stack<FunctionData> _functionsStack = new();
    private readonly ImportsManager _importsManager = new();
    private List<FunctionData> _functions = [];

    private BytecodeFunction CurFunction => _functionsStack.Peek().BytecodeFunction;
    private List<BytecodeInstruction> CurBytecode => CurFunction.Code.Instructions;

    public BytecodeModule Translate(AsgNode root)
    {
        var getter = new PrecompileDataGetter();
        _functions = getter.GetFunctions(root);
        Visit(root);
        return new BytecodeModule("Program", _functions.Select(x => x.BytecodeFunction).ToList());
    }

    // split to classes
    private void Visit(AsgNode node)
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
                break;
            case AsgNodeType.FunctionCreating:
                _functionsStack.Push(_functions.First(x => x.BytecodeFunction.Name == node.Text));
                DefineVariables(_functionsStack.Peek().Parameters);
                DefineVariables(_functionsStack.Peek().Locals);
                CurBytecode.AddRange(
                    SimpleBytecodeGenerator.ReadParameters(node.Children[1].Children.Select(c => c.Text).ToArray())
                );
                Visit(node.Children[^1]);
                break;
            case AsgNodeType.SetOperation:
                Visit(node.Children[^1]);
                var varName = node.Children[0].Text;
                CurBytecode.Add(new BytecodeInstruction(InstructionType.SetLocal, [varName]));
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
                    CurBytecode.Add(new BytecodeInstruction(InstructionType.CallFunc, [functionName]));
                }

                break;
            case AsgNodeType.If:
                SimpleBytecodeGenerator.If(
                    () => Visit(node.Children[0]),
                    () => Visit(node.Children[1]),
                    CurFunction.Code
                );
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
                SimpleBytecodeGenerator.For(
                    () => Visit(node.Children[0]),
                    () => Visit(node.Children[1]),
                    () => Visit(node.Children[2]),
                    () => Visit(node.Children[3]),
                    CurFunction.Code
                );
                break;
            case AsgNodeType.WhileLoop:
                break;
            case AsgNodeType.Multiplication:
                Operation(node, MathLogicOp.Mul);
                break;
            case AsgNodeType.Division:
                Operation(node, MathLogicOp.Div);
                break;
            case AsgNodeType.Addition:
                Operation(node, MathLogicOp.Sum);
                break;
            case AsgNodeType.Subtraction:
                Operation(node, MathLogicOp.Sub);
                break;
            case AsgNodeType.LessThan:
                Operation(node, MathLogicOp.Lt);
                break;
            case AsgNodeType.GreaterThan:
                Operation(node, MathLogicOp.Gt);
                break;
            case AsgNodeType.LessThanOrEqual:
                Operation(node, MathLogicOp.LtOrEq);
                break;
            case AsgNodeType.GreaterThanOrEqual:
                Operation(node, MathLogicOp.GtOrEq);
                break;
            case AsgNodeType.Equal:
                Operation(node, MathLogicOp.Eq);
                break;
            case AsgNodeType.NotEqual:
                Operation(node, MathLogicOp.NotEq);
                break;
            case AsgNodeType.Modulus:
                Operation(node, MathLogicOp.Mod);
                break;
            case AsgNodeType.Scope:
                Visit(node.Children);
                break;
            case AsgNodeType.Power:
                Operation(node, MathLogicOp.Pow);
                break;
            case AsgNodeType.And:
                Operation(node, MathLogicOp.And);
                break;
            case AsgNodeType.Or:
                Operation(node, MathLogicOp.Or);
                break;
            case AsgNodeType.Xor:
                Operation(node, MathLogicOp.Xor);
                break;
            case AsgNodeType.Not:
                Operation(node, MathLogicOp.Not);
                break;
            case AsgNodeType.Identifier:
                CurBytecode.Add(new BytecodeInstruction(InstructionType.LoadLocal, [node.Text]));
                break;
            case AsgNodeType.BrIf:
                Visit(node.Children[0]);
                CurBytecode.Add(new BytecodeInstruction(
                        InstructionType.BrOp,
                        [BranchMode.IfTrue.ToAny(), node.Children[1].Text]
                    )
                );
                break;
            case AsgNodeType.Label:
                CurBytecode.Add(new BytecodeInstruction(InstructionType.Label, [node.Text]));
                break;
            case AsgNodeType.MaxEnumValue:
            default:
                Throw.InvalidOpEx();
                break;
        }
    }

    private void DefineVariables(List<BytecodeVariable> variables)
    {
        CurBytecode.AddRange(SimpleBytecodeGenerator.DefineLocals(variables.Select(x => (x.Name, x.Type)).ToArray()));
    }

    private void Operation(AsgNode node, MathLogicOp mathLogicOp)
    {
        Visit(node.Children);
        CurBytecode.Add(new BytecodeInstruction(InstructionType.MathOrLogicOp, [mathLogicOp.ToAny()]));
    }

    private static string GetString(string text) => text[1..^1];

    private void Visit(List<AsgNode> nodeChildren)
    {
        foreach (var node in nodeChildren)
            Visit(node);
    }
}