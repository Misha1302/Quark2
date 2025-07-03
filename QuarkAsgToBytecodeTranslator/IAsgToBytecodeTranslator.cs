namespace AsgToBytecodeTranslator;

public interface IAsgToBytecodeTranslator<T>
{
    public BytecodeModule Translate(AsgNode<T> root,
        Func<AsgToBytecodeData<T>, bool> asgBuilderExtensionMethod = null!);

    public void Visit(AsgNode<T> node);
}