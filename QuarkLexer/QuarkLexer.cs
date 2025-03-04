using Lexer;

namespace DefaultLexerImpl;

public class QuarkLexer(LexerConfiguration<QuarkLexemeType> configuration)
    : DefaultLexer<QuarkLexemeType>(configuration);