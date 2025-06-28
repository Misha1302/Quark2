using System.Reflection;
using ExceptionsManager;
using GenericBytecode2.Structures;

namespace GenericBytecode2;

public record InstructionAction
{
    private readonly Func<string>? _delegateToString;

    private readonly Lazy<ParameterInfo[]> _parametersLazy;
    public readonly Delegate Action;

    /// <summary>
    ///     Action must be:
    ///     1. single method <br />
    ///     2. return type = void <br />
    ///     3. parameters = (out? p)* <br />
    ///     4. static <br />
    /// </summary>
    /// <param name="action"></param>
    /// <param name="delegateToString"></param>
    public InstructionAction(Delegate action, Func<string>? delegateToString = null)
    {
        CheckAction(action);

        Action = action;
        _delegateToString = delegateToString;

        _parametersLazy = new Lazy<ParameterInfo[]>(Action.Method.GetParameters);
    }

    public ParameterInfo[] Parameters => _parametersLazy.Value;

    private static void CheckAction(Delegate action)
    {
        CheckIsSingleAction(action);
        CheckReturnType(action);
        CheckParameters(action);
        CheckStatic(action);
    }

    private static void CheckStatic(Delegate action)
    {
        Throw.AssertAlways(action.Method.IsStatic, $"Action ({action.Method.Name}) must be static");
    }

    private static void CheckParameters(Delegate action)
    {
        Throw.AssertAlways(
            action.Method.GetParameters().All(Predicate),
            "Action must take (out) parameters of (sub)type IBasicValue"
        );
        return;

        bool Predicate(ParameterInfo p)
        {
            var t = p.ParameterType.GetElementType() ?? p.ParameterType;
            var r = typeof(IBasicValue).IsAssignableFrom(t) || t == typeof(IBasicValue);
            return r;
        }
    }

    private static void CheckReturnType(Delegate action)
    {
        Throw.AssertAlways(
            action.Method.ReturnType == typeof(void),
            "Action must have no return value"
        );
    }

    private static void CheckIsSingleAction(Delegate action)
    {
        Throw.AssertAlways(
            action.GetInvocationList().Length == 1,
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