namespace QuarkCFrontend;

public static class QuarkLexerDefaultConfiguration
{
    public static LexerConfiguration<QuarkLexemeType> CreateDefault()
    {
        var patterns =
            (List<LexemePattern<QuarkLexemeType>>)
            [
                new LexemePattern<QuarkLexemeType>("//.*", Comment),
                new LexemePattern<QuarkLexemeType>(@"\(", LeftPar),
                new LexemePattern<QuarkLexemeType>(@"\)", RightPar),
                new LexemePattern<QuarkLexemeType>(@"\{", LeftBrace),
                new LexemePattern<QuarkLexemeType>(@"\}", RightBrace),
                new LexemePattern<QuarkLexemeType>(@"\[", LeftBracket),
                new LexemePattern<QuarkLexemeType>(@"\]", RightBracket),
                new LexemePattern<QuarkLexemeType>(@"\<\=", Le),
                new LexemePattern<QuarkLexemeType>(@"\>\=", Ge),
                new LexemePattern<QuarkLexemeType>(@"\<", Lt),
                new LexemePattern<QuarkLexemeType>("def", Def),
                new LexemePattern<QuarkLexemeType>("and", And),
                new LexemePattern<QuarkLexemeType>("or", Or),
                new LexemePattern<QuarkLexemeType>("not", Not),
                new LexemePattern<QuarkLexemeType>(@"\>", Gt),
                new LexemePattern<QuarkLexemeType>(@"\=\=", EqEq),
                new LexemePattern<QuarkLexemeType>(@"\=", Eq),
                new LexemePattern<QuarkLexemeType>(@"\!\=", Neq),
                new LexemePattern<QuarkLexemeType>(@"\+", Addition),
                new LexemePattern<QuarkLexemeType>(@"\-", Subtraction),
                new LexemePattern<QuarkLexemeType>(@"\*\*", Power),
                new LexemePattern<QuarkLexemeType>(@"\*", Multiplication),
                new LexemePattern<QuarkLexemeType>(@"\/", Division),
                new LexemePattern<QuarkLexemeType>(@"\%", Modulus),
                new LexemePattern<QuarkLexemeType>("if", If),
                new LexemePattern<QuarkLexemeType>("else", Else),
                new LexemePattern<QuarkLexemeType>("elif", ElseIf),
                new LexemePattern<QuarkLexemeType>("for", For),
                new LexemePattern<QuarkLexemeType>("while", While),
                new LexemePattern<QuarkLexemeType>("return", Return),
                new LexemePattern<QuarkLexemeType>("import", Import),
                new LexemePattern<QuarkLexemeType>(@"[0-9]+(\.[0-9]+)?", Number),
                new LexemePattern<QuarkLexemeType>("\".*?\"", QuarkLexemeType.String),
                new LexemePattern<QuarkLexemeType>("brif", BrIf),
                new LexemePattern<QuarkLexemeType>("[a-zA-Z_][a-zA-Z0-9_]*", Identifier),
                new LexemePattern<QuarkLexemeType>("@[a-zA-Z_][a-zA-Z0-9_]*", Label),
                new LexemePattern<QuarkLexemeType>("[ \n\t]+", WhiteSpace),
            ];

        var lexemesToIgnore = (List<QuarkLexemeType>) [WhiteSpace];

        var config = new LexerConfiguration<QuarkLexemeType>(patterns, lexemesToIgnore);

        return config;
    }
}