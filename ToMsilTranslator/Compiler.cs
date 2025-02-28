using RuntimeHackes;

namespace ToMsilTranslator;

public class Compiler
{
    public IEnumerable<CompiledMethod> Compile(List<DynamicMethod> methods) =>
        methods.Select(method => new CompiledMethod(method.Name, method.GetNativePointer()));

    public record CompiledMethod(string Name, nint Pointer);
}