using AbstractExecutor;
using AsgToBytecodeTranslator;
using DefaultAstImpl.Asg;
using DefaultLexerImpl;
using QuarkCFrontend;

const string code =
    """
    import "../../../../Libraries"

    def Main() {
        n = 132749
        top = n ** (1 / 2) + 1
        isPrime = IsPrimeRec(n, top, 2)
        _ = PrintLn(isPrime)
        return 0
    }

    def IsPrimeRec(n, top, i) {
        if (i >= top) { return 1 }
        if (n % i == 0) { return 0 }

        return IsPrimeRec(n, top, i + 1)
    }
    """;

var lexemes = new Lexer(QuarkLexerDefaultConfiguration.CreateDefault()).Lexemize(code);
var asg = new AsgBuilder<QuarkLexemeType>(QuarkAsgBuilderConfiguration.CreateDefault()).Build(lexemes);
var module = new AsgToBytecodeTranslator<QuarkLexemeType>().Translate(asg);
var executor = new TranslatorToMsil.TranslatorToMsil();
executor.Init(new ExecutorConfiguration(module));
var results = executor.RunModule();
Console.WriteLine(results.First());