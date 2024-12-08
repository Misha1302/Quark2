namespace QuarkCFrontend;

public record AsgNode(AsgNodeType NodeType, LexemeValue LexemeValue, List<AsgNode> Children)
{
    public LexemeType? LexemeType => LexemeValue?.LexemePattern.LexemeType;
    public AsgNodeType NodeType { get; set; } = NodeType;
    public string Text => LexemeValue.Text;

    public override string ToString() => ToStringCustom(0);

    private string ToStringCustom(int offset)
    {
        var s = new string(' ', offset);
        return
            $"{s}{NodeType}: {LexemeValue} : [\n{string.Join("\n", Children.Select(x => x.ToStringCustom(offset + 4)))}\n{s}]";
    }
}