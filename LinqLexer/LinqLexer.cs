using System.Text.RegularExpressions;
using CommonFrontendApi;

namespace LinqFrontend;

public class LinqLexer(LexerConfiguration<LinqLexemeType> configuration)
{
    // Method that performs lexical analysis on the input code and returns a list of tokens.
    public List<LexemeValue<LinqLexemeType>> Lexemize(string code)
    {
        var lexemes = (List<LexemeValue<LinqLexemeType>>) [];

        var ind = 0;
        while (ind < code.Length)
        {
            var curStr = configuration.Patterns
                .Select(x => (match: Regex.Match(code[ind..], x.Pattern), x))
                .FirstOrDefault(x => x.match is { Success: true, Index: 0 });

            if (curStr == default)
            {
                if (lexemes.Count == 0 || lexemes[^1].LexemePattern.LexemeType != LinqLexemeType.SomeUnknownText)
                    lexemes.Add(new LexemeValue<LinqLexemeType>("",
                        new LexemePattern<LinqLexemeType>("", LinqLexemeType.SomeUnknownText), ind));
                lexemes[^1].Text += code[ind];
                ind++;
            }
            else
            {
                lexemes.Add(new LexemeValue<LinqLexemeType>(curStr.match.Value, curStr.x, ind));
                ind += curStr.match.Length;
            }
        }

        return lexemes;
    }
}