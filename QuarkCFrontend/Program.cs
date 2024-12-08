using QuarkCFrontend.Asg;
using QuarkCFrontend.Lexer;

const string code =
    """
    import "Libraries"

    Number Main() {
       Number n = ReadNumber()
    
       Number delimitersCount = 0
    
        Print(n)
        if IsPrime(number) {
            PrintLn(" is prime")
        }
        else {
            PrintLn(" is not prime")
        }
    
        return 0
    }

    Bool IsPrime(Number n) {
        for Number i = 2; i * i <= n; i = i + 1 {
            if n % i == 0 {
                return false
            }
        }
        return n >= 2
    }
    """;

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