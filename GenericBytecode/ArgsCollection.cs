using System.Collections;

namespace GenericBytecode;

public class ArgsCollection : IEnumerable<InstructionAction>
{
    private readonly List<InstructionAction> _begin = [];
    private readonly List<InstructionAction> _end = [];
    private readonly List<InstructionAction> _middle = [];

    private int _prevHash;
    private List<InstructionAction> _prevTotal = null!;

    public List<InstructionAction> Total
    {
        get
        {
            if (
                _prevHash == 0 ||
                _prevTotal.GetHashCode() != (_begin.GetHashCode() ^ _middle.GetHashCode() ^ _end.GetHashCode())
            )
                _prevTotal = [.._begin, .._middle, .._end];

            _prevHash = _prevTotal.GetHashCode();
            return _prevTotal;
        }
    }

    public InstructionAction this[int i] => Total[i];

    public IEnumerator<InstructionAction> GetEnumerator() => Total.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void AddToBegin(InstructionAction action) => _begin.Insert(0, action);
    public void AddToEnd(InstructionAction action) => _end.Add(action);
    public void AddToMiddle(InstructionAction action) => _middle.Add(action);

    public static implicit operator ArgsCollection(Span<InstructionAction> actions)
    {
        var collection = new ArgsCollection();
        foreach (var action in actions)
            collection.AddToBegin(action);
        return collection;
    }

    public static implicit operator ArgsCollection(InstructionAction action)
    {
        var collection = new ArgsCollection();
        collection.AddToBegin(action);
        return collection;
    }
}