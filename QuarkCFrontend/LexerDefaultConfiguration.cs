using QuarkCFrontend.Lexer;

namespace QuarkCFrontend;

public static class LexerDefaultConfiguration
{
    public static LexerConfiguration CreateDefault()
    {
        var patterns =
            (List<LexemePattern>)
            [
                new LexemePattern("//.*", LexemeType.Comment),
                new LexemePattern(@"\(", LexemeType.LeftPar),
                new LexemePattern(@"\)", LexemeType.RightPar),
                new LexemePattern(@"\{", LexemeType.LeftBrace),
                new LexemePattern(@"\}", LexemeType.RightBrace),
                new LexemePattern(@"\[", LexemeType.LeftBracket),
                new LexemePattern(@"\]", LexemeType.RightBracket),
                new LexemePattern(@"\<\=", LexemeType.Le),
                new LexemePattern(@"\>\=", LexemeType.Ge),
                new LexemePattern(@"\<", LexemeType.Lt),
                new LexemePattern(@"def", LexemeType.Def),
                new LexemePattern(@"and", LexemeType.And),
                new LexemePattern(@"or", LexemeType.Or),
                new LexemePattern(@"not", LexemeType.Not),
                new LexemePattern(@"\>", LexemeType.Gt),
                new LexemePattern(@"\=\=", LexemeType.EqEq),
                new LexemePattern(@"\=", LexemeType.Eq),
                new LexemePattern(@"\!\=", LexemeType.Neq),
                new LexemePattern(@"\+", LexemeType.Addition),
                new LexemePattern(@"\-", LexemeType.Subtraction),
                new LexemePattern(@"\*\*", LexemeType.Power),
                new LexemePattern(@"\*", LexemeType.Multiplication),
                new LexemePattern(@"\/", LexemeType.Division),
                new LexemePattern(@"\%", LexemeType.Modulus),
                new LexemePattern("if", LexemeType.If),
                new LexemePattern("else", LexemeType.Else),
                new LexemePattern("elif", LexemeType.ElseIf),
                new LexemePattern("for", LexemeType.For),
                new LexemePattern("while", LexemeType.While),
                new LexemePattern("return", LexemeType.Return),
                new LexemePattern("import", LexemeType.Import),
                new LexemePattern(@"[0-9]+(\.[0-9]+)?", LexemeType.Number),
                new LexemePattern("\".*?\"", LexemeType.String),
                new LexemePattern("brif", LexemeType.BrIf),
                new LexemePattern("[a-zA-Z_][a-zA-Z0-9_]*", LexemeType.Identifier),
                new LexemePattern("@[a-zA-Z_][a-zA-Z0-9_]*", LexemeType.Label),
                new LexemePattern("[ \n\t]+", LexemeType.WhiteSpace),
            ];

        var lexemesToIgnore = (List<LexemeType>) [LexemeType.WhiteSpace];

        var config = new LexerConfiguration(patterns, lexemesToIgnore);

        return config;
    }
}