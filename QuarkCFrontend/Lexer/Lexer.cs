using System.Text.RegularExpressions;

namespace QuarkCFrontend.Lexer;

public class Lexer(LexerConfiguration configuration)
{
    public List<LexemeValue> Lexemize(string code)
    {
        var allMatches = (List<LexemeValue>) [];

        foreach (var pattern in configuration.Patterns)
            allMatches.AddRange(
                Regex.Matches(code, pattern.Pattern)
                    .Select(x => new LexemeValue(x.Value, pattern, x.Index, code))
            );

        allMatches = allMatches
            .OrderBy(x => x.StartIndex)
            .ThenBy(x => configuration.Patterns.IndexOf(x.LexemePattern))
            .ToList();

        var result = new List<LexemeValue>();
        var index = 0;
        var prevFoundIndex = 0;
        while (index < code.Length)
        {
            // ReSharper disable once AccessToModifiedClosure
            (var lexeme, prevFoundIndex) = allMatches.FirstOptimized(x => x.StartIndex >= index, prevFoundIndex);
            // var lexeme = allMatches.First(x => x.StartIndex >= index);
            index = lexeme.StartIndex + lexeme.Text.Length;

            if (!configuration.LexemesToIgnore.Contains(lexeme.LexemePattern.LexemeType))
                result.Add(lexeme);
        }

        return result;
    }
}