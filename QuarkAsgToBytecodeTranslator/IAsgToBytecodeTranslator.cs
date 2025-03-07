namespace AsgToBytecodeTranslator;

public interface IAsgToBytecodeTranslator<T>
{
    public void Visit(AsgNode<T> node);
}