namespace LinqLexer;

public static class LinqLexerDefaultConfiguration
{
    public static LexerConfiguration<LinqLexemeType> CreateDefault()
    {
        var patterns =
            (List<LexemePattern<LinqLexemeType>>)
            [
                new LexemePattern<LinqLexemeType>("use", LinqLexemeType.Use),
                new LexemePattern<LinqLexemeType>("over", LinqLexemeType.Over),
                new LexemePattern<LinqLexemeType>("all", LinqLexemeType.All),
                new LexemePattern<LinqLexemeType>("any", LinqLexemeType.Any),
                new LexemePattern<LinqLexemeType>("skip", LinqLexemeType.Skip),
                new LexemePattern<LinqLexemeType>("select", LinqLexemeType.Select),
                new LexemePattern<LinqLexemeType>("average", LinqLexemeType.Average),
                new LexemePattern<LinqLexemeType>("count", LinqLexemeType.Count),
                new LexemePattern<LinqLexemeType>("sum", LinqLexemeType.Sum),
                new LexemePattern<LinqLexemeType>("mul", LinqLexemeType.Mul),
                new LexemePattern<LinqLexemeType>("first", LinqLexemeType.First),
                new LexemePattern<LinqLexemeType>("sort", LinqLexemeType.Sort),
                new LexemePattern<LinqLexemeType>("reverse", LinqLexemeType.Reverse),
                new LexemePattern<LinqLexemeType>("where", LinqLexemeType.Where),
                new LexemePattern<LinqLexemeType>("last", LinqLexemeType.Last),
                new LexemePattern<LinqLexemeType>("end", LinqLexemeType.End),
            ];

        var lexemesToIgnore = (List<LinqLexemeType>) [LinqLexemeType.WhiteSpace];

        var config = new LexerConfiguration<LinqLexemeType>(patterns, lexemesToIgnore);

        return config;
    }
}