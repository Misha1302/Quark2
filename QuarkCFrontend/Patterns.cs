using QuarkCFrontend.Lexer;

namespace QuarkCFrontend;

public static class Patterns
{
    private static readonly List<LexemePattern> _patterns =
    [
        new(@"\(", LexemeType.LeftPar),
        new(@"\)", LexemeType.RightPar),
        new(@"\{", LexemeType.LeftBrace),
        new(@"\}", LexemeType.RightBrace),
        new(@"\[", LexemeType.LeftBracket),
        new(@"\]", LexemeType.RightBracket),
        new(@"\<\=", LexemeType.Le),
        new(@"\>\=", LexemeType.Ge),
        new(@"\<", LexemeType.Lt),
        new(@"and", LexemeType.And),
        new(@"or", LexemeType.Or),
        new(@"not", LexemeType.Not),
        new(@"\>", LexemeType.Gt),
        new(@"\=\=", LexemeType.EqEq),
        new(@"\=", LexemeType.Eq),
        new(@"\!\=", LexemeType.Neq),
        new(@"\+", LexemeType.Addition),
        new(@"\-", LexemeType.Subtraction),
        new(@"\*\*", LexemeType.Power),
        new(@"\*", LexemeType.Multiplication),
        new(@"\/", LexemeType.Division),
        new(@"\%", LexemeType.Modulus),
        new("if", LexemeType.If),
        new("else", LexemeType.Else),
        new("elif", LexemeType.ElseIf),
        new("for", LexemeType.For),
        new("while", LexemeType.While),
        new("return", LexemeType.Return),
        new("import", LexemeType.Import),
        new(@"[0-9]+(\.[0-9]+)?", LexemeType.Number),
        new("\".*?\"", LexemeType.String),
        new("[a-zA-Z_][a-zA-Z0-9_]*", LexemeType.Identifier),
    ];

    public static IReadOnlyList<LexemePattern> GetPatterns() => _patterns;

    public static void AddNewPattern(LexemePattern pattern, int index) => _patterns.Insert(index, pattern);
}