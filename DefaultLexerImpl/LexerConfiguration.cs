using DefaultLexerImpl.Lexer;

namespace DefaultLexerImpl;

public record LexerConfiguration(List<LexemePattern> Patterns, List<LexemeType> LexemesToIgnore)
{
    // TODO: add global DI
    // public static LexerConfiguration Default => LexerDefaultConfiguration.CreateDefault();
}