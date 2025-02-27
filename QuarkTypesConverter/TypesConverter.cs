using System.Globalization;
using SharpAnyType;

namespace QuarkTypesConverter;

public static class TypesConverter
{
    public static Any ToStr(Any any) => any.ToString();
    public static Any ToNumber(Any any) => Convert.ToDouble(any.ToString(), CultureInfo.InvariantCulture);
}