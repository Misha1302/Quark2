using System.Reflection;
using CommonArrayPool;
using CommonExtensions;
using ExceptionsManager;
using GenericBytecode.Interfaces;

namespace GenericBytecode.Instruction;

public record InstructionAction
{
    private readonly Func<string>? _delegateToString;

    private readonly Lazy<ParameterInfo[]> _parametersLazy;
    private readonly Lazy<ParameterInfo[]> _parametersRefsLazy;
    private readonly Lazy<ParameterInfo[]> _parametersWithoutRefsLazy;

    public readonly Delegate Action;

    /// <summary>
    ///     Action must be: <br />
    ///     1. single method <br />
    ///     2. return type = void <br />
    ///     3. parameters = (out? p)* <br />
    ///     4. static <br />
    ///     5. types of return (out) parameters must be classes <br />
    /// </summary>
    /// <param name="action"></param>
    /// <param name="delegateToString"></param>
    public InstructionAction(Delegate action, Func<string>? delegateToString = null)
    {
        Action = action;
        _delegateToString = delegateToString;

        _parametersLazy = new Lazy<ParameterInfo[]>(Action.Method.GetParameters);
        _parametersWithoutRefsLazy =
            new Lazy<ParameterInfo[]>(() => Parameters.Where(p => !p.ParameterType.IsByRef).ToArray());
        _parametersRefsLazy =
            new Lazy<ParameterInfo[]>(() => Parameters.Where(p => p.ParameterType.IsByRef).ToArray());

        CheckAction();
    }

    public ParameterInfo[] Parameters => _parametersLazy.Value;
    public ParameterInfo[] ParametersWithoutRefs => _parametersWithoutRefsLazy.Value;
    public ParameterInfo[] ParametersRefs => _parametersRefsLazy.Value;

    public static implicit operator InstructionAction(Delegate action) => new(action);

    private void CheckAction()
    {
        CheckIsSingleAction();
        CheckReturnType();
        CheckParameters();
        CheckReturnParameters();
        CheckStatic();
    }

    private void CheckReturnParameters()
    {
        Throw.AssertAlways(
            ParametersRefs.All(p => p.ParameterType.IsClass),
            "Types of out parameters (return values) must be specific class"
        );
    }

    private void CheckStatic()
    {
        Throw.AssertAlways(Action.Method.IsStatic, $"Action ({Action.Method.Name}) must be static");
    }

    private void CheckParameters()
    {
        Throw.AssertAlways(
            ParametersRefs.All(IsImplement<IBasicValue>),
            $"Action must take (out) parameters of type which implements {nameof(IBasicValue)}"
        );

        return;

        bool IsImplement<T>(ParameterInfo p) =>
            (p.ParameterType.GetElementType() ?? p.ParameterType).IsImplement<T>();
    }

    private void CheckReturnType()
    {
        Throw.AssertAlways(
            Action.Method.ReturnType == typeof(void),
            "Action must have no return value"
        );
    }

    private void CheckIsSingleAction()
    {
        Throw.AssertAlways(
            Action.GetInvocationList().Length == 1,
            "Action must be single"
        );
    }

    public void Invoke(params object?[] args)
    {
        Action.Method.Invoke(null, args);
    }

    public T Invoke<T>(Span<object?> args = default)
    {
        var arr = GenericArrayPool<object?>.Shared.Rent(args.Length + 1);
        Array.Clear(arr);
        args.UnsafeCopyTo(arr);

        Action.Method.Invoke(null, arr);

        var res = (T)arr[^1]!;
        GenericArrayPool<object?>.Shared.Return(arr);
        return res;
    }

    public override string ToString() => (_delegateToString?.Invoke() ?? Action.Method.ToString()) ?? string.Empty;
}