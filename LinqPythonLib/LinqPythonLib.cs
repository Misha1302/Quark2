using LinqFrontend;

namespace LinqPythonLib;

public class LinqPythonLib
{
    public void Run(string code, Action<string> outputAction)
    {
        var lexemes = new LinqLexer(LinqLexerDefaultConfiguration.CreateDefault()).Lexemize(code);
        var output = new AsgToTextTranslator().Translate(lexemes);

        outputAction(output);
    }
}