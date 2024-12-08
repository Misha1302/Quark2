using System.Text.RegularExpressions;

namespace QuarkCFrontend.Lexer;

public class Lexer
{
    private readonly List<LexemePattern> patterns = Patterns.GetPatterns().ToList();

    public List<LexemeValue> Lexemize(string code)
    {
        var allMatches = (List<LexemeValue>) [];

        foreach (var pattern in patterns)
            allMatches.AddRange(
                Regex.Matches(code, pattern.Pattern)
                    .Select(x => new LexemeValue(x.Value, pattern, x.Index))
            );

        allMatches = allMatches
            .OrderBy(x => x.StartIndex)
            .ThenBy(x => patterns.IndexOf(x.LexemePattern))
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
            result.Add(lexeme);
        }

        return result;
    }
}