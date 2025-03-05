namespace DefaultAstImpl.Asg;

public record AsgNode<T>(
    AsgNodeType NodeType,
    LexemeValue<T>? LexemeValue,
    List<AsgNode<T>> Children,
    int BaseLineNumber = -1)
{
    public AsgNodeType NodeType = NodeType;

    // ReSharper disable once ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
    public T? LexemeType => LexemeValue != null ? LexemeValue.LexemePattern.LexemeType : default;
    public string Text => LexemeValue?.Text ?? "";

    public int LineNumber => BaseLineNumber == -1 ? LexemeValue!.LineNumber : BaseLineNumber;

    public override string ToString() => ToStringCustom(0);

    private string ToStringCustom(int offset)
    {
        var s = new string(' ', offset);
        return
            $"{s}{NodeType}: {LexemeValue} : [\n{string.Join("\n", Children.Select(x => x.ToStringCustom(offset + 4)))}\n{s}]";
    }
}