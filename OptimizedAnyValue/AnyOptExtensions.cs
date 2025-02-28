namespace DynamicStrongTypeValue;

public static class AnyOptExtensions
{
    public static object GetValueInSharpType(this AnyOpt value)
    {
        return value.Type switch
        {
            Nil => "Nil",
            Number => value.Get<double>(),
            Str => value.GetRef<string>(),
            SomeSharpObject => value.GetRef<object>(),
            NativeI64 => value.Get<long>(),
            AnyValueType.Any => Throw.InvalidOpEx<object>("Cannot convert value of type Any to sharp object"),
            _ => Throw.InvalidOpEx<object>($"Unknown Any type ({value.Type})"),
        };
    }

    public static AnyOpt MakeAnyOpt(this Any any)
    {
        return any.Value switch
        {
            double d => AnyOpt.Create(d, Number),
            long l => AnyOpt.Create(l, NativeI64),
            bool l => AnyOpt.Create(l ? 1.0 : 0.0, Number),
            string s => AnyOpt.CreateRef(s, Str),
            Enum e => AnyOpt.Create((long)Convert.ToInt32(e), NativeI64),
            null => AnyOpt.NilValue,
            var obj => AnyOpt.CreateRef(obj, SomeSharpObject),
        };
    }
}