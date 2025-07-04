using System.Linq.Expressions;
using System.Reflection;
using CommonFrontendApi;
using DefaultAstImpl.Asg;
using ExceptionsManager;
using GenericBytecode;
using GenericBytecode.Instruction;
using WistExtensions;
using static GenericBytecode.Instruction.InstructionManager;

namespace BasicMathExtension;

public class BasicMathExtension : IWistExtension
{
    public static readonly InstructionValue AddInstruction = GetNextInstruction(nameof(AddInstruction));
    public static readonly InstructionValue DivInstruction = GetNextInstruction(nameof(DivInstruction));
    public static readonly InstructionValue ModInstruction = GetNextInstruction(nameof(ModInstruction));
    public static readonly InstructionValue MulInstruction = GetNextInstruction(nameof(MulInstruction));
    public static readonly InstructionValue SubInstruction = GetNextInstruction(nameof(SubInstruction));

    private static readonly InstructionValue[] _instructions =
        [AddInstruction, SubInstruction, MulInstruction, DivInstruction, ModInstruction];

    public LexerConfiguration<T> ExtendLexerConfiguration<T>(LexerConfiguration<T> configuration) =>
        configuration;

    public AsgBuilderConfiguration<T> ExtendParserConfiguration<T>(AsgBuilderConfiguration<T> configuration) =>
        configuration;

    public AsgNode<T> ManipulateAst<T>(AsgNode<T> root) =>
        root;

    public GenericBytecodeModule ManipulateBytecode(GenericBytecodeModule module)
    {
        foreach (var function in module.Functions)
            for (var i = 0; i < function.Body.Instructions.Count; i++)
            {
                var instruction = function.Body.Instructions[i];
                if (instruction.Value.IsSomeOf(_instructions))
                    HandleInstruction(function, i);
            }

        return module;
    }

    private void HandleInstruction(GenericBytecodeFunction function, int instrIndex)
    {
        var instr = function.Body.Instructions[instrIndex];
        var type = function.GetTypesStack(instrIndex)[^1];
        var addF = GetMethodInfoToCall(instr).MakeGenericMethod(type);

        var parameters = addF.GetParameters().Select(x => x.ParameterType).ToArray();
        var argsAndRet = parameters.Append(typeof(void)).ToArray();
        var @delegate = Delegate.CreateDelegate(Expression.GetDelegateType(argsAndRet), addF);
        instr.Args.AddToEnd(@delegate);
    }

    private static MethodInfo GetMethodInfoToCall(Instruction instr)
    {
        var name =
            instr.Value == AddInstruction
                ? nameof(MathExtFunctions.AddFunc)
                : instr.Value == SubInstruction
                    ? nameof(MathExtFunctions.SubFunc)
                    : instr.Value == DivInstruction
                        ? nameof(MathExtFunctions.DivFunc)
                        : instr.Value == MulInstruction
                            ? nameof(MathExtFunctions.MulFunc)
                            : instr.Value == ModInstruction
                                ? nameof(MathExtFunctions.ModFunc)
                                : Throw.InvalidOpEx<string>();

        return typeof(MathExtFunctions).GetMethod(name)!;
    }
}