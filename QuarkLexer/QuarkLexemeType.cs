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
    Dot
}

public static class QuarkLexemeTypeHelper
{
    private static long _num = (long)QuarkLexemeType.WhiteSpace;

    public static QuarkLexemeType GetNextFreeNumber() => (QuarkLexemeType)(++_num);
}