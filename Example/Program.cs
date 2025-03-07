using AbstractExecutor;
using AsgToBytecodeTranslator;
using DefaultAstImpl.Asg;
using DefaultLexerImpl;
using QuarkCFrontend;
using QuarkStructures;

const string code =
    """
    import "../../../../Libraries"

    def Main() {
    }
    """;

var extensions = (List<QuarkExtStructures>) [new QuarkExtStructures()];

var lexerConf = QuarkLexerDefaultConfiguration.CreateDefault();
foreach (var ext in extensions) lexerConf = ext.ExtendLexerConfiguration(lexerConf);
var lexemes = new QuarkLexer(lexerConf).Lexemize(code);
Console.WriteLine(string.Join("\n", lexemes));

var parserConf = QuarkAsgBuilderConfiguration.CreateDefault();
foreach (var ext in extensions) parserConf = ext.ExtendAsgBuilderConfiguration(parserConf);
var asg = new AsgBuilder<QuarkLexemeType>(parserConf).Build(lexemes);
Console.WriteLine(asg);

var translatorHandlers = (Action<AsgToBytecodeData<QuarkLexemeType>>)null!;
foreach (var ext in extensions) translatorHandlers += ext.GetUnknownAsgCodeHandler();
var module = new AsgToBytecodeTranslator<QuarkLexemeType>().Translate(asg, translatorHandlers);
Console.WriteLine(module);

var executor = new TranslatorToMsil.TranslatorToMsil();
executor.Init(new ExecutorConfiguration(module));

var results = executor.RunModule();
Console.WriteLine(results.First());