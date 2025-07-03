using System.Linq.Expressions;
using CommonFrontendApi;
using DefaultAstImpl.Asg;
using ExceptionsManager;
using GenericBytecode;
using WistExtensions;
using static GenericBytecode.InstructionManager;

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
        if (instr.Value != AddInstruction) Throw.NotImplementedException();

        var type = function.GetTypesStack(instrIndex)[^1];
        var addF = typeof(MathExtFunctions).GetMethod(nameof(MathExtFunctions.AddFunc))!.MakeGenericMethod(type);

        var parameters = addF.GetParameters().Select(x => x.ParameterType).ToArray();
        var argsAndRet = parameters.Append(typeof(void)).ToArray();
        var @delegate = Delegate.CreateDelegate(Expression.GetDelegateType(argsAndRet), addF);
        instr.Args.AddToEnd(@delegate);
    }
}