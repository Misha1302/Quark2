using CommonBytecode.Data.AnyValue;

namespace AbstractExecutor;

public record ExecutorConfiguration(Dictionary<string, Action<Any>> BuildInFunctions);