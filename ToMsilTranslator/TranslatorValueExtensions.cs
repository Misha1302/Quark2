namespace ToMsilTranslator;

public static class TranslatorValueExtensions
{
    public static object GetValueInSharpType(this TranslatorValue value)
    {
        return value.Type switch
        {
            Nil => "Nil",
            Number => value.Get<double>(),
            Str => value.GetRef<string>(),
            SomeSharpObject => value.GetRef<object>(),
            NativeI64 => value.Get<long>(),
            BytecodeValueType.Any => value.Get<long>(),
            _ => Throw.InvalidOpEx<string>(),
        };
    }

    public static TranslatorValue MakeTranslationValue(this Any any)
    {
        return any.Value switch
        {
            double d => TranslatorValue.Create(d, Number),
            long l => TranslatorValue.Create(l, NativeI64),
            bool l => TranslatorValue.Create(l ? 1L : 0L, NativeI64),
            string s => TranslatorValue.CreateRef(s, Str),
            Enum e => TranslatorValue.Create((long)Convert.ToInt32(e), NativeI64),
            null => TranslatorValue.NilValue,
            var obj => TranslatorValue.CreateRef(obj, SomeSharpObject),
        };
    }
}