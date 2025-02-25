namespace CommonFrontendApi;

public record LexemeValue<T>(string Text, LexemePattern<T> LexemePattern, int StartIndex)
{
    private readonly Lazy<int>? _lineNumber;
    public string Text = Text;

    public LexemeValue(string text, LexemePattern<T> lexemePattern, int startIndex, string code)
        : this(text, lexemePattern, startIndex)
    {
        _lineNumber = new Lazy<int>(() => CalcLineNumber(code, StartIndex));
    }

    public int LineNumber => _lineNumber?.Value ?? -1;

    private static int CalcLineNumber(string code, int startIndex)
    {
        var lineNumber = code.Take(startIndex).Count(c => c == '\n') + 1;
        return lineNumber;
    }
}