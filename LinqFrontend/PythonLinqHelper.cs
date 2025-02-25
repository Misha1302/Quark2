using System.Text;
using CommonFrontendApi;
using LinqLexer;

namespace LinqFrontend;

public static class PythonLinqHelper
{
    public static string GetTextLinqPreliminaryFunctions(string executeName, string ansName) =>
        $"""
         def {executeName}(code):
             exec(code)
             return {ansName}
         """ + "\n\n\n";

    public static string CreateLoopInitializer(string xName, string iName) =>
        $"{xName} = None\n" +
        $"{iName} = 0\n";

    public static string CreateExpression(List<LexemeValue<LinqLexemeType>> lexemes, int index, StringBuilder sbTop)
    {
        var expr = lexemes[index + 1].Text;
        if (!expr.Trim().StartsWith('{')) return expr;

        var clauseName = CreateUniqueName();

        sbTop.AppendLine(
            $"""
             def {clauseName}():
                 {expr.Trim(' ', '{', '}')}
             """
        );

        return $"{clauseName}()";
    }

    public static string CreateUniqueName() =>
        "_" + string.Join("", Guid.NewGuid().ToByteArray().Select(x => x.ToString("x2")));

    public static bool IsPythonKeyword(string s) =>
        s
            is "and"
            or "as"
            or "assert"
            or "break"
            or "class"
            or "continue"
            or "def"
            or "del"
            or "elif"
            or "else"
            or "except"
            or "False"
            or "finally"
            or "for"
            or "from"
            or "global"
            or "if"
            or "import"
            or "in"
            or "is"
            or "lambda"
            or "None"
            or "nonlocal"
            or "not"
            or "or"
            or "pass"
            or "raise"
            or "return"
            or "True"
            or "try"
            or "while"
            or "with"
            or "yield";
}