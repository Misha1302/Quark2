using DefaultLexerImpl.Lexer;
using ExceptionsManager;

namespace LinqFrontend;

public class AsgToTextTranslator
{
    private readonly string _ansName = PythonLinqHelper.CreateUniqueName();
    private readonly string _executeName = PythonLinqHelper.CreateUniqueName();

    public string Translate(List<LexemeValue> lexemes, bool intoOneLine = false)
    {
        var output = "";

        output += PythonLinqHelper.GetTextLinqPreliminaryFunctions(_executeName, _ansName);

        for (var index = 0; index < lexemes.Count; index++)
        {
            var lexeme = lexemes[index];
            if (lexeme.LexemePattern.LexemeType == LexemeType.Use)
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


    private string MakeLinq(List<LexemeValue> lexemes, ref int index)
    {
        var linqCreator = new PythonLinqCreator(_ansName, lexemes);

        linqCreator.MakeGlobalIdentifiers(index);

        linqCreator.CreateAnswer();

        while (true)
        {
            var lexeme = lexemes[index];
            var type = lexeme.LexemePattern.LexemeType;

            if (type == LexemeType.Use)
                linqCreator.Use(ref index);
            else if (type == LexemeType.Over)
                linqCreator.Over(ref index);
            else if (type == LexemeType.Select)
                linqCreator.Select(ref index);
            else if (type == LexemeType.Where)
                linqCreator.Where(ref index);
            else if (type == LexemeType.All)
                linqCreator.All(ref index);
            else if (type is LexemeType.Count)
                linqCreator.Count(ref index);
            else if (type is LexemeType.Sum)
                linqCreator.Sum(ref index);
            else if (type is LexemeType.Mul)
                linqCreator.Mul(ref index);
            else if (type == LexemeType.Skip)
                linqCreator.Skip(ref index);
            else if (type == LexemeType.First)
                linqCreator.First(ref index);
            else if (type == LexemeType.Last)
                linqCreator.Last(ref index);
            else if (type == LexemeType.End)
                break;
            else Throw.NotImplementedException($"This ({type}) operation was not implemented.");

            index++;
        }

        return _executeName + "(" + "\"\"\"" + linqCreator.Concat() + "\"\"\"" + ")";
    }
}