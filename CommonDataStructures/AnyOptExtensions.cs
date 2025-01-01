using CommonBytecode.Data.AnyValue;
using ExceptionsManager;

namespace CommonDataStructures;

public static class AnyOptExtensions
{
    public static object GetValueInSharpType(this AnyOpt value)
    {
        return value.Type switch
        {
            BytecodeValueType.Nil => "Nil",
            BytecodeValueType.Number => value.Get<double>(),
            BytecodeValueType.Str => value.GetRef<string>(),
            BytecodeValueType.SomeSharpObject => value.GetRef<object>(),
            BytecodeValueType.NativeI64 => value.Get<long>(),
            BytecodeValueType.Any => value.Get<long>(),
            _ => Throw.InvalidOpEx<string>(),
        };
    }

    public static AnyOpt MakeAnyOpt(this Any any)
    {
        return any.Value switch
        {
            double d => AnyOpt.Create(d, BytecodeValueType.Number),
            long l => AnyOpt.Create(l, BytecodeValueType.NativeI64),
            bool l => AnyOpt.Create(l ? 1L : 0L, BytecodeValueType.NativeI64),
            string s => AnyOpt.CreateRef(s, BytecodeValueType.Str),
            Enum e => AnyOpt.Create((long)Convert.ToInt32(e), BytecodeValueType.NativeI64),
            null => AnyOpt.NilValue,
            var obj => AnyOpt.CreateRef(obj, BytecodeValueType.SomeSharpObject),
        };
    }
}