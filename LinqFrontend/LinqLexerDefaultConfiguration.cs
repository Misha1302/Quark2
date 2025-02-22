using DefaultLexerImpl;
using DefaultLexerImpl.Lexer;

namespace LinqFrontend;

public static class LinqLexerDefaultConfiguration
{
    public static LexerConfiguration CreateDefault()
    {
        var patterns =
            (List<LexemePattern>)
            [
                new LexemePattern("use", LexemeType.Use),
                new LexemePattern("over", LexemeType.Over),
                new LexemePattern("all", LexemeType.All),
                new LexemePattern("any", LexemeType.Any),
                new LexemePattern("skip", LexemeType.Skip),
                new LexemePattern("select", LexemeType.Select),
                new LexemePattern("average", LexemeType.Average),
                new LexemePattern("count", LexemeType.Count),
                new LexemePattern("sum", LexemeType.Sum),
                new LexemePattern("mul", LexemeType.Mul),
                new LexemePattern("first", LexemeType.First),
                new LexemePattern("sort", LexemeType.Sort),
                new LexemePattern("reverse", LexemeType.Reverse),
                new LexemePattern("where", LexemeType.Where),
                new LexemePattern("last", LexemeType.Last),
                new LexemePattern("end", LexemeType.End),
            ];

        var lexemesToIgnore = (List<LexemeType>) [LexemeType.WhiteSpace];

        var config = new LexerConfiguration(patterns, lexemesToIgnore);

        return config;
    }
}