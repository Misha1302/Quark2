using CommonFrontendApi;
using DefaultLexerImpl;

namespace QuarkCFrontend;

public static class QuarkLexerDefaultConfiguration
{
    public static LexerConfiguration<QuarkLexemeType> CreateDefault()
    {
        var patterns =
            (List<LexemePattern<QuarkLexemeType>>)
            [
                new LexemePattern<QuarkLexemeType>("//.*", QuarkLexemeType.Comment),
                new LexemePattern<QuarkLexemeType>(@"\(", QuarkLexemeType.LeftPar),
                new LexemePattern<QuarkLexemeType>(@"\)", QuarkLexemeType.RightPar),
                new LexemePattern<QuarkLexemeType>(@"\{", QuarkLexemeType.LeftBrace),
                new LexemePattern<QuarkLexemeType>(@"\}", QuarkLexemeType.RightBrace),
                new LexemePattern<QuarkLexemeType>(@"\[", QuarkLexemeType.LeftBracket),
                new LexemePattern<QuarkLexemeType>(@"\]", QuarkLexemeType.RightBracket),
                new LexemePattern<QuarkLexemeType>(@"\<\=", QuarkLexemeType.Le),
                new LexemePattern<QuarkLexemeType>(@"\>\=", QuarkLexemeType.Ge),
                new LexemePattern<QuarkLexemeType>(@"\<", QuarkLexemeType.Lt),
                new LexemePattern<QuarkLexemeType>("def", QuarkLexemeType.Def),
                new LexemePattern<QuarkLexemeType>("and", QuarkLexemeType.And),
                new LexemePattern<QuarkLexemeType>("or", QuarkLexemeType.Or),
                new LexemePattern<QuarkLexemeType>("not", QuarkLexemeType.Not),
                new LexemePattern<QuarkLexemeType>(@"\>", QuarkLexemeType.Gt),
                new LexemePattern<QuarkLexemeType>(@"\=\=", QuarkLexemeType.EqEq),
                new LexemePattern<QuarkLexemeType>(@"\=", QuarkLexemeType.Eq),
                new LexemePattern<QuarkLexemeType>(@"\!\=", QuarkLexemeType.Neq),
                new LexemePattern<QuarkLexemeType>(@"\+", QuarkLexemeType.Addition),
                new LexemePattern<QuarkLexemeType>(@"\-", QuarkLexemeType.Subtraction),
                new LexemePattern<QuarkLexemeType>(@"\*\*", QuarkLexemeType.Power),
                new LexemePattern<QuarkLexemeType>(@"\*", QuarkLexemeType.Multiplication),
                new LexemePattern<QuarkLexemeType>(@"\/", QuarkLexemeType.Division),
                new LexemePattern<QuarkLexemeType>(@"\%", QuarkLexemeType.Modulus),
                new LexemePattern<QuarkLexemeType>("if", QuarkLexemeType.If),
                new LexemePattern<QuarkLexemeType>("else", QuarkLexemeType.Else),
                new LexemePattern<QuarkLexemeType>("elif", QuarkLexemeType.ElseIf),
                new LexemePattern<QuarkLexemeType>("for", QuarkLexemeType.For),
                new LexemePattern<QuarkLexemeType>("while", QuarkLexemeType.While),
                new LexemePattern<QuarkLexemeType>("return", QuarkLexemeType.Return),
                new LexemePattern<QuarkLexemeType>("import", QuarkLexemeType.Import),
                new LexemePattern<QuarkLexemeType>(@"[0-9]+(\.[0-9]+)?", QuarkLexemeType.Number),
                new LexemePattern<QuarkLexemeType>("\".*?\"", QuarkLexemeType.String),
                new LexemePattern<QuarkLexemeType>("brif", QuarkLexemeType.BrIf),
                new LexemePattern<QuarkLexemeType>("[a-zA-Z_][a-zA-Z0-9_]*", QuarkLexemeType.Identifier),
                new LexemePattern<QuarkLexemeType>("@[a-zA-Z_][a-zA-Z0-9_]*", QuarkLexemeType.Label),
                new LexemePattern<QuarkLexemeType>("[ \n\t]+", QuarkLexemeType.WhiteSpace),
            ];

        var lexemesToIgnore = (List<QuarkLexemeType>) [QuarkLexemeType.WhiteSpace];

        var config = new LexerConfiguration<QuarkLexemeType>(patterns, lexemesToIgnore);

        return config;
    }
}