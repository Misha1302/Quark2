using GenericBytecode;

namespace WistExtensions;

public interface IWistBytecodeExtension
{
    public GenericBytecodeModule ManipulateBytecode(GenericBytecodeModule module);
}