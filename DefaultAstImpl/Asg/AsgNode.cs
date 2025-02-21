using DefaultLexerImpl.Lexer;

namespace DefaultAstImpl.Asg;

public record AsgNode(AsgNodeType NodeType, LexemeValue LexemeValue, List<AsgNode> Children, int BaseLineNumber = -1)
{
    // ReSharper disable once ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
    public LexemeType? LexemeType => LexemeValue?.LexemePattern.LexemeType;
    public AsgNodeType NodeType { get; set; } = NodeType;
    public string Text => LexemeValue.Text;

    public int LineNumber => BaseLineNumber == -1 ? LexemeValue.LineNumber : BaseLineNumber;

    public override string ToString() => ToStringCustom(0);

    private string ToStringCustom(int offset)
    {
        var s = new string(' ', offset);
        return
            $"{s}{NodeType}: {LexemeValue} : [\n{string.Join("\n", Children.Select(x => x.ToStringCustom(offset + 4)))}\n{s}]";
    }
}