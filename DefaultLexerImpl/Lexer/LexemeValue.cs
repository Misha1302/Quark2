namespace DefaultLexerImpl.Lexer;

public record LexemeValue(string Text, LexemePattern LexemePattern, int StartIndex)
{
    private readonly Lazy<int> _lineNumber = null!;

    public LexemeValue(string text, LexemePattern lexemePattern, int startIndex, string code)
        : this(text, lexemePattern, startIndex)
    {
        _lineNumber = new Lazy<int>(() => CalcLineNumber(code, StartIndex));
    }

    public int LineNumber => _lineNumber.Value;

    public static int CalcLineNumber(string code, int startIndex)
    {
        var lineNumber = code.Take(startIndex).Count(c => c == '\n') + 1;
        return lineNumber;
    }
}