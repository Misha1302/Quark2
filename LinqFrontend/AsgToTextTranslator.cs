using CommonFrontendApi;
using ExceptionsManager;

namespace LinqFrontend;

public class AsgToTextTranslator
{
    private readonly string _ansName = PythonLinqHelper.CreateUniqueName();
    private readonly string _executeName = PythonLinqHelper.CreateUniqueName();

    public string Translate(List<LexemeValue<LinqLexemeType>> lexemes, bool intoOneLine = false)
    {
        var output = "";

        output += PythonLinqHelper.GetTextLinqPreliminaryFunctions(_executeName, _ansName);

        for (var index = 0; index < lexemes.Count; index++)
        {
            var lexeme = lexemes[index];
            if (lexeme.LexemePattern.LexemeType == LinqLexemeType.Use)
            {
                var linqCode = MakeLinq(lexemes, ref index);
                output += intoOneLine ? linqCode.Replace("\n", "\\n") : linqCode;
            }
            else
            {
                output += lexeme.Text;
            }
        }

        return output;
    }


    private string MakeLinq(List<LexemeValue<LinqLexemeType>> lexemes, ref int index)
    {
        var linqCreator = new PythonLinqCreator(_ansName, lexemes);

        linqCreator.MakeGlobalIdentifiers(index);

        linqCreator.CreateAnswer();

        while (true)
        {
            var lexeme = lexemes[index];
            var type = lexeme.LexemePattern.LexemeType;

            if (type == LinqLexemeType.Use)
                linqCreator.Use(ref index);
            else if (type == LinqLexemeType.Over)
                linqCreator.Over(ref index);
            else if (type == LinqLexemeType.Select)
                linqCreator.Select(ref index);
            else if (type == LinqLexemeType.Where)
                linqCreator.Where(ref index);
            else if (type == LinqLexemeType.All)
                linqCreator.All(ref index);
            else if (type is LinqLexemeType.Count)
                linqCreator.Count(ref index);
            else if (type is LinqLexemeType.Sum)
                linqCreator.Sum(ref index);
            else if (type is LinqLexemeType.Mul)
                linqCreator.Mul(ref index);
            else if (type == LinqLexemeType.Skip)
                linqCreator.Skip(ref index);
            else if (type == LinqLexemeType.First)
                linqCreator.First(ref index);
            else if (type == LinqLexemeType.Sort)
                linqCreator.Sort(ref index);
            else if (type == LinqLexemeType.Reverse)
                linqCreator.Reverse(ref index);
            else if (type == LinqLexemeType.Last)
                linqCreator.Last(ref index);
            else if (type == LinqLexemeType.End)
                break;
            else Throw.NotImplementedException($"This ({type}) operation was not implemented.");

            index++;
        }

        return _executeName + "(" + "\"\"\"" + linqCreator.Concat() + "\"\"\"" + ")";
    }
}