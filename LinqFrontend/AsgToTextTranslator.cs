using System.Text;
using DefaultLexerImpl.Lexer;
using ExceptionsManager;

namespace LinqFrontend;

public class AsgToTextTranslator
{
    private readonly string _ansName = CreateUniqueName();
    private readonly string _executeName = CreateUniqueName();

    public string Translate(List<LexemeValue> lexemes, bool intoOneLine = false)
    {
        var output = "";

        output += GetTextLinqPreliminaryFunctions();

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

    private string GetTextLinqPreliminaryFunctions() =>
        $"""
         def {_executeName}(code):
             exec(code)
             return {_ansName}
         """ + "\n\n\n";

    private string MakeLinq(List<LexemeValue> lexemes, ref int index)
    {
        // TODO: convert into class
        var identifiersToSetGlobal = (List<string>) [];
        var sbTop = new StringBuilder();
        var sbMiddle = new StringBuilder();
        var sbBottom = new StringBuilder();
        var offset = new StringBuilder();

        var xName = "";
        var iName = "";
        var overName = "";

        for (var i = index; lexemes[i].LexemePattern.LexemeType != LexemeType.End; i++)
            identifiersToSetGlobal.AddRange(lexemes[i].Text.Split('(', ')', '[', ']', ' ', '\t'));

        identifiersToSetGlobal = identifiersToSetGlobal
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Where(x => x.All(c => char.IsLetter(c) || c == '_'))
            .Where(x => !IsPythonKeyword(x))
            .ToList();

        foreach (var identifier in identifiersToSetGlobal)
            sbTop.AppendLine($"try: global {identifier}\nexcept: pass");

        sbTop.AppendLine($"global {_ansName}");
        sbTop.AppendLine($"{_ansName} = None");

        while (true)
        {
            var lexeme = lexemes[index];
            var type = lexeme.LexemePattern.LexemeType;
            if (type == LexemeType.Use)
            {
                var q = lexemes[index + 1].Text.Split(",").Select(x => x.Trim()).ToArray();
                (xName, iName) = (q[0], q[1]);
                index++;

                sbTop.AppendLine(CreateLoopInitializer(xName, iName));
            }
            else if (type == LexemeType.Over)
            {
                overName = CreateUniqueName();
                sbTop.AppendLine($"{overName} = ({lexemes[index + 1].Text})");
                index++;
            }
            else if (type == LexemeType.Select)
            {
                var expr = CreateExpression(lexemes, index, ref sbTop);
                var resultCollection = CreateUniqueName();
                var topName = CreateUniqueName();

                sbMiddle.AppendLine(
                    $"""
                     {CreateLoopInitializer(xName, iName)}
                     {offset}
                     {topName} = len({overName})
                     {resultCollection} = []

                     while {iName} < {topName}:
                        {xName} = {overName}[{iName}]
                        {resultCollection}.append({expr}) 
                        {iName} += 1
                        
                     {_ansName} = {resultCollection}
                     {overName} = {_ansName}
                     """
                );

                offset.Clear();


                index++;
            }
            else if (type == LexemeType.Where)
            {
                var expr = CreateExpression(lexemes, index, ref sbTop);
                var resultCollection = CreateUniqueName();
                var topName = CreateUniqueName();

                sbMiddle.AppendLine(
                    $"""
                     {CreateLoopInitializer(xName, iName)}
                     {offset}
                     {topName} = len({overName})
                     {resultCollection} = []

                     while {iName} < {topName}:
                        {xName} = {overName}[{iName}]
                        if ({expr}):
                            {resultCollection}.append({xName}) 
                        {iName} += 1
                        
                     {_ansName} = {resultCollection}
                     {overName} = {_ansName}
                     """
                );

                offset.Clear();

                index++;
            }
            else if (type == LexemeType.All)
            {
                var expr = CreateExpression(lexemes, index, ref sbTop);
                var topName = CreateUniqueName();
                var accumulator = CreateUniqueName();

                sbMiddle.AppendLine(
                    $"""
                     {CreateLoopInitializer(xName, iName)}
                     {offset}
                     {topName} = len({overName})
                     {accumulator} = True

                     while {iName} < {topName}:
                        {xName} = {overName}[{iName}]
                        {accumulator} &= ({expr})
                        {iName} += 1

                     {_ansName} = {accumulator}
                     """
                );

                offset.Clear();

                index++;
            }
            else if (type is LexemeType.Count or LexemeType.Sum or LexemeType.Mul)
            {
                var expr = CreateExpression(lexemes, index, ref sbTop);
                var topName = CreateUniqueName();
                var accumulator = CreateUniqueName();

                sbMiddle.AppendLine(
                    $"""
                     {CreateLoopInitializer(xName, iName)}
                     {offset}
                     {topName} = len({overName})
                     {accumulator} = {(type == LexemeType.Mul ? "1" : "0")}

                     while {iName} < {topName}:
                        {xName} = {overName}[{iName}]
                        {accumulator} {(type != LexemeType.Mul ? "+" : "*")}= ({expr})
                        {iName} += 1

                     {_ansName} = {accumulator}
                     """
                );

                offset.Clear();

                index++;
            }
            else if (type == LexemeType.Skip)
            {
                offset.AppendLine($"{iName} += ({lexemes[index + 1].Text})");
                index++;
            }
            else if (type == LexemeType.First)
            {
                sbMiddle.AppendLine($"{_ansName} = {_ansName}[0]");
                index++;
            }
            else if (type == LexemeType.Last)
            {
                sbMiddle.AppendLine($"{_ansName} = {_ansName}[-1]");
                index++;
            }
            else if (type == LexemeType.End)
            {
                break;
            }
            else
            {
                Throw.NotImplementedException($"This ({type}) operation was not implemented.");
            }

            index++;
        }

        var str = $"{sbTop}\n{sbMiddle}\n{sbBottom}";
        return _executeName + "(" + "\"\"\"" + str + "\"\"\"" + ")";
    }

    private bool IsPythonKeyword(string s) =>
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

    private static string CreateLoopInitializer(string xName, string iName) =>
        $"{xName} = None\n" +
        $"{iName} = 0\n";

    private string CreateExpression(List<LexemeValue> lexemes, int index, ref StringBuilder sbTop)
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

    private static string CreateUniqueName() =>
        "_" + string.Join("", Guid.NewGuid().ToByteArray().Select(x => x.ToString("x2")));
}