using QuarkCFrontend.Lexer;

namespace QuarkCFrontend;

public record LexerConfiguration(List<LexemePattern> Patterns, List<LexemeType> LexemesToIgnore)
{
    public static LexerConfiguration Default => LexerDefaultConfiguration.CreateDefault();
}