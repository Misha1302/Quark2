using CommonBytecode.Data.AnyValue;
using StdDict = QuarkMap.QuarkMapImpl<CommonBytecode.Data.AnyValue.Any, CommonBytecode.Data.AnyValue.Any>;

namespace QuarkMap;

public static class QuarkMapOperations
{
    public static Any GetMapValue(Any dict, Any key) => dict.Get<StdDict>().Get(key);

    public static void SetMapValue(Any dict, Any key, Any value) => dict.Get<StdDict>().Set(key, value);
    
    public static void RemoveMapValue(Any dict, Any key) => dict.Get<StdDict>().Remove(key);

    public static Any CreateMap() => new(new StdDict()) { Type = BytecodeValueType.SomeSharpObject };
}