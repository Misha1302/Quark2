namespace DefaultLexerImpl;

public enum QuarkLexemeType
{
    // ReSharper disable once UnusedMember.Global
    Unknown,
    Identifier,
    LeftPar,
    RightPar,
    LeftBrace,
    RightBrace,
    Eq,
    Lt,
    Gt,
    Le,
    Ge,
    Neq,
    If,
    Else,
    ElseIf,
    For,
    While,
    Return,
    Number,
    String,
    Import,
    LeftBracket,
    RightBracket,
    EqEq,
    Addition,
    Subtraction,
    Multiplication,
    Division,
    Modulus,
    Power,
    And,
    Or,
    Xor,
    Not,
    Label,
    BrIf,
    Comment,
    Def,
    WhiteSpace,
    Dot,

    // after this enum value should not exist other values to correct work QuarkLexemeTypeHelper  
    MaxQuarkLexeme,
}

public static class QuarkLexemeTypeHelper
{
    private static long _num = (long)QuarkLexemeType.MaxQuarkLexeme;

    public static QuarkLexemeType GetNextFreeNumber() => (QuarkLexemeType)(++_num);
}

public static class QuarkLexemeTypeExtensions
{
    public static bool IsNeedBlock(this QuarkLexemeType type) =>
        type is QuarkLexemeType.If or QuarkLexemeType.Else or QuarkLexemeType.ElseIf
            or QuarkLexemeType.Def or QuarkLexemeType.While or QuarkLexemeType.For;
}