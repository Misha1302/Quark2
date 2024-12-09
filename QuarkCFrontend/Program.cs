using QuarkCFrontend.Asg;
using QuarkCFrontend.Lexer;

const string code2 =
    """
    import "Libraries"

    Number Main() {
        return 0
    }
    """;

var lexemes = new Lexer().Lexemize(code2);
Console.WriteLine(string.Join("\n", lexemes));

var asg = new AsgBuilder(AsgBuilderConfiguration.Default).Build(lexemes);

Console.WriteLine(string.Join("\n", asg));