using AbstractExecutor;
using AsgToBytecodeTranslator;
using DefaultAstImpl.Asg;
using DefaultLexerImpl;
using QuarkCFrontend;

const string code =
    """
    import "../../../../Libraries"

    def Main() {
        s = ""
        for (n = 0) (n < 10) (n = n + 1) {
            if n % 2 == 0 { s = Concat(s, "D2 ") }
            elif n % 3 == 0 { s = Concat(s, "D3 ") }
            else { s = Concat(s, "Hi ") }
        }
        return s
    }
    """;

var lexemes = new QuarkLexer(QuarkLexerDefaultConfiguration.CreateDefault()).Lexemize(code);
var asg = new AsgBuilder<QuarkLexemeType>(QuarkAsgBuilderConfiguration.CreateDefault()).Build(lexemes);
var module = new AsgToBytecodeTranslator<QuarkLexemeType>().Translate(asg);
var executor = new TranslatorToMsil.TranslatorToMsil();
executor.Init(new ExecutorConfiguration(module));
var results = executor.RunModule();
Console.WriteLine(results.First());