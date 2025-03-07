namespace AsgToBytecodeTranslator;

public interface IAsgToBytecodeTranslator<T>
{
    public BytecodeModule Translate(AsgNode<T> root, Action<AsgToBytecodeData<T>> asgBuilderExtensionMethod = null!);

    public void Visit(AsgNode<T> node);
}