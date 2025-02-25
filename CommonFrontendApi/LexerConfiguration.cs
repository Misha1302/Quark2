namespace CommonFrontendApi;

public record LexerConfiguration<T>(List<LexemePattern<T>> Patterns, List<T> LexemesToIgnore);