using CommonBytecode.Data.AnyValue;
using ExceptionsManager;

namespace ToMsilTranslator;

public static class TranslatorValueExtensions
{
    public static object GetValueInSharpType(this TranslatorValue value)
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

    public static TranslatorValue MakeTranslationValue(this Any any)
    {
        return any.Value switch
        {
            double d => TranslatorValue.Create(d, BytecodeValueType.Number),
            long l => TranslatorValue.Create(l, BytecodeValueType.NativeI64),
            bool l => TranslatorValue.Create(l ? 1L : 0L, BytecodeValueType.NativeI64),
            string s => TranslatorValue.CreateRef(s, BytecodeValueType.Str),
            Enum e => TranslatorValue.Create((long)Convert.ToInt32(e), BytecodeValueType.NativeI64),
            null => TranslatorValue.NilValue,
            var obj => TranslatorValue.CreateRef(obj, BytecodeValueType.SomeSharpObject),
        };
    }
}