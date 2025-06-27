using System.Reflection;
using ExceptionsManager;

namespace GenericBytecode2;

public record InstructionAction
{
    private readonly Func<string>? _delegateToString;
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
    }

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

    public override string ToString() => (_delegateToString?.Invoke() ?? Action.Method.ToString()) ?? string.Empty;
}