namespace CommonDataStructures;

public interface IReadOnlyStack<out T>
{
    public T Get(int ind);
}