namespace DefaultLexerImpl;

public class QuarkLexemeTypes
{
    public static readonly QuarkLexemeType Identifier = 0;
    public static readonly QuarkLexemeType LeftPar = 1;
    public static readonly QuarkLexemeType RightPar = 2;
    public static readonly QuarkLexemeType LeftBrace = 3;
    public static readonly QuarkLexemeType RightBrace = 4;
    public static readonly QuarkLexemeType Eq = 5;
    public static readonly QuarkLexemeType Lt = 6;
    public static readonly QuarkLexemeType Gt = 7;
    public static readonly QuarkLexemeType Le = 8;
    public static readonly QuarkLexemeType Ge = 9;
    public static readonly QuarkLexemeType Neq = 10;
    public static readonly QuarkLexemeType If = 11;
    public static readonly QuarkLexemeType Else = 12;
    public static readonly QuarkLexemeType ElseIf = 13;
    public static readonly QuarkLexemeType For = 14;
    public static readonly QuarkLexemeType While = 15;
    public static readonly QuarkLexemeType Return = 16;
    public static readonly QuarkLexemeType Number = 17;
    public static readonly QuarkLexemeType String = 18;
    public static readonly QuarkLexemeType Import = 19;
    public static readonly QuarkLexemeType LeftBracket = 20;
    public static readonly QuarkLexemeType RightBracket = 21;
    public static readonly QuarkLexemeType EqEq = 22;
    public static readonly QuarkLexemeType Addition = 23;
    public static readonly QuarkLexemeType Subtraction = 24;
    public static readonly QuarkLexemeType Multiplication = 25;
    public static readonly QuarkLexemeType Division = 26;
    public static readonly QuarkLexemeType Modulus = 27;
    public static readonly QuarkLexemeType Power = 28;
    public static readonly QuarkLexemeType And = 29;
    public static readonly QuarkLexemeType Or = 30;
    public static readonly QuarkLexemeType Xor = 31;
    public static readonly QuarkLexemeType Not = 32;
    public static readonly QuarkLexemeType Label = 33;
    public static readonly QuarkLexemeType BrIf = 34;
    public static readonly QuarkLexemeType Comment = 35;
    public static readonly QuarkLexemeType Def = 36;
    public static readonly QuarkLexemeType WhiteSpace = 37;
}

public record QuarkLexemeType(long Value)
{
    public static implicit operator QuarkLexemeType(long l) => new(l);

    public static implicit operator long(QuarkLexemeType t) => t.Value;
}