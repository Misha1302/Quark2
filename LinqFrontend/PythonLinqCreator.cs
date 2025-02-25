using System.Text;
using CommonFrontendApi;

namespace LinqFrontend;

public class PythonLinqCreator(string ansName, List<LexemeValue<LinqLexemeType>> lexemes)
{
    private readonly StringBuilder _offset = new();
    private readonly StringBuilder _sbBottom = new();
    private readonly StringBuilder _sbMiddle = new();
    private readonly StringBuilder _sbTop = new();

    private string _iName = "";
    private string _overName = "";
    private string _xName = "";

    public void MakeGlobalIdentifiers(int index)
    {
        var identifiersToSetGlobal = (List<string>) [];

        for (var i = index; lexemes[i].LexemePattern.LexemeType != LinqLexemeType.End; i++)
            identifiersToSetGlobal.AddRange(lexemes[i].Text.Split('(', ')', '[', ']', ' ', '\t'));

        identifiersToSetGlobal = identifiersToSetGlobal
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Where(x => x.All(c => char.IsLetter(c) || c == '_'))
            .Where(x => !PythonLinqHelper.IsPythonKeyword(x))
            .ToList();

        foreach (var identifier in identifiersToSetGlobal)
            _sbTop.AppendLine($"try: global {identifier}\nexcept: pass");
    }

    public void CreateAnswer()
    {
        _sbTop.AppendLine($"global {ansName}");
        _sbTop.AppendLine($"{ansName} = None");
    }

    public void Use(ref int index)
    {
        var q = lexemes[index + 1].Text.Split(",").Select(x => x.Trim()).ToArray();
        (_xName, _iName) = (q[0], q[1]);
        index++;

        _sbTop.AppendLine(PythonLinqHelper.CreateLoopInitializer(_xName, _iName));
    }

    public void Over(ref int index)
    {
        _overName = PythonLinqHelper.CreateUniqueName();
        _sbTop.AppendLine($"{ansName} = ({_overName} := ({lexemes[index + 1].Text}))");
        index++;
    }

    public void Select(ref int index)
    {
        MakeLoopInstruction(ref index, Loop,
            (resultCollection, _) => $"{resultCollection} = []",
            (_, _) => $"{_overName} = {ansName}"
        );
        return;

        string Loop(string topName, string expr, string resultCollection)
        {
            return $"""
                    while {_iName} < {topName}:
                       {_xName} = {_overName}[{_iName}]
                       {resultCollection}.append({expr}) 
                       {_iName} += 1
                    """;
        }
    }

    public void Where(ref int index)
    {
        MakeLoopInstruction(ref index, Loop,
            (resultCollection, _) => $"{resultCollection} = []",
            (_, _) => $"{_overName} = {ansName}"
        );
        return;

        string Loop(string topName, string expr, string resultCollection)
        {
            return $"""
                    while {_iName} < {topName}:
                    {_xName} = {_overName}[{_iName}]
                    if ({expr}):
                        {resultCollection}.append({_xName}) 
                    {_iName} += 1
                    """;
        }
    }

    public void All(ref int index)
    {
        MakeLoopInstruction(ref index, Loop,
            (accumulator, _) => $"{accumulator} = True",
            (_, _) => $"{_overName} = {ansName}"
        );
        return;

        string Loop(string topName, string expr, string accumulator)
        {
            return $"""
                    while {_iName} < {topName}:
                       {_xName} = {_overName}[{_iName}]
                       {accumulator} &= ({expr})
                       {_iName} += 1
                    """;
        }
    }

    public void Any(ref int index)
    {
        MakeLoopInstruction(ref index, Loop,
            (accumulator, _) => $"{accumulator} = False",
            (_, _) => $"{_overName} = {ansName}"
        );
        return;

        string Loop(string topName, string expr, string accumulator)
        {
            return $"""
                    while {_iName} < {topName}:
                       {_xName} = {_overName}[{_iName}]
                       {accumulator} |= ({expr})
                       if {accumulator}: break
                       {_iName} += 1
                    """;
        }
    }

    public void Count(ref int index)
    {
        MakeLoopInstruction(ref index, Loop,
            (accumulator, _) => $"{accumulator} = 0",
            (_, _) => $"{_overName} = {ansName}"
        );
        return;

        string Loop(string topName, string expr, string accumulator)
        {
            return $"""
                    while {_iName} < {topName}:
                       {_xName} = {_overName}[{_iName}]
                       {accumulator} += ({expr})
                       {_iName} += 1
                    """;
        }
    }

    public void Sum(ref int index)
    {
        Count(ref index);
    }

    public void Mul(ref int index)
    {
        MakeLoopInstruction(ref index, Loop,
            (accumulator, _) => $"{accumulator} = 1",
            (_, _) => $"{_overName} = {ansName}"
        );
        return;

        string Loop(string topName, string expr, string accumulator)
        {
            return $"""
                    while {_iName} < {topName}:
                       {_xName} = {_overName}[{_iName}]
                       {accumulator} *= ({expr})
                       {_iName} += 1
                    """;
        }
    }

    private void MakeLoopInstruction(ref int index,
        Func<string, string, string, string> loop,
        Func<string, string, string> start,
        Func<string, string, string> end
    )
    {
        var expr = PythonLinqHelper.CreateExpression(lexemes, index, _sbTop);
        var result = PythonLinqHelper.CreateUniqueName();
        var topName = PythonLinqHelper.CreateUniqueName();

        _sbMiddle.AppendLine(
            $"""
             {PythonLinqHelper.CreateLoopInitializer(_xName, _iName)}
             {_offset}
             {topName} = len({_overName})
             {"\n" + start(result, topName) + "\n"}

             {loop(topName, expr, result).Replace("\n", "\n    ")}
                
             {ansName} = {result}
             {"\n" + end(result, topName) + "\n"}
             """);

        _offset.Clear();

        index++;
    }

    public void Skip(ref int index)
    {
        _offset.AppendLine($"{_iName} += ({lexemes[index + 1].Text})");
        index++;
    }

    public void First(ref int index)
    {
        _sbMiddle.AppendLine($"{ansName} = {ansName}[0]");
        index++;
    }

    public void Sort(ref int index)
    {
        _sbMiddle.AppendLine($"{ansName} = sorted({ansName})");
        index++;
    }

    public void Reverse(ref int index)
    {
        _sbMiddle.AppendLine($"reverse({ansName})");
        index++;
    }

    public void Last(ref int index)
    {
        _sbMiddle.AppendLine($"{ansName} = {ansName}[-1]");
        index++;
    }

    public string Concat() => $"{_sbTop}\n{_sbMiddle}\n{_sbBottom}";
}