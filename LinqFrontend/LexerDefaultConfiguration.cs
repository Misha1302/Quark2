using DefaultLexerImpl;
using DefaultLexerImpl.Lexer;

namespace LinqFrontend;

public static class LexerDefaultConfiguration
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
                new LexemePattern("average", LexemeType.Average),
                new LexemePattern("count", LexemeType.Count),
                new LexemePattern("first", LexemeType.First),
                new LexemePattern("where", LexemeType.Where),
                new LexemePattern(@"\(", LexemeType.LeftBrace),
                new LexemePattern(@"\)", LexemeType.RightBrace),
                new LexemePattern("[ \n\t]+", LexemeType.WhiteSpace),
            ];

        var lexemesToIgnore = (List<LexemeType>) [LexemeType.WhiteSpace];

        var config = new LexerConfiguration(patterns, lexemesToIgnore);

        return config;
    }
}