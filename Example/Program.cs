using AbstractExecutor;
using AsgToBytecodeTranslator;
using DefaultAstImpl.Asg;
using DefaultLexerImpl;
using QuarkCFrontend;
using QuarkStructures;

const string code =
    """
    import "../../../../Libraries"

    struct Vector3(x, y, z)

    def Main() {
        v = CreateStruct("Vector3")
        v->x = 3
        v->y = 34.42
        v->z = v->y / v->x
        _ = PrintLn(v)
        return 0
    }
    """;

var extensions = (List<QuarkExtStructures>) [new QuarkExtStructures()];

var lexerConf = QuarkLexerDefaultConfiguration.CreateDefault();
lexerConf = extensions.Aggregate(lexerConf, (current, ext) => ext.ExtendLexerConfiguration(current));
var lexemes = new QuarkLexer(lexerConf).Lexemize(code);
Console.WriteLine(string.Join("\n", lexemes));

var parserConf = QuarkAsgBuilderConfiguration.CreateDefault();
parserConf = extensions.Aggregate(parserConf, (current, ext) => ext.ExtendAsgBuilderConfiguration(current));
var asg = new AsgBuilder<QuarkLexemeType>(parserConf).Build(lexemes);
Console.WriteLine(asg);

var translatorHandlers = extensions.Aggregate((Action<AsgToBytecodeData<QuarkLexemeType>>)null!,
    (current, ext) => current + ext.GetUnknownAsgCodeHandler());
var module = new AsgToBytecodeTranslator<QuarkLexemeType>().Translate(asg, translatorHandlers);
Console.WriteLine(module);

var executor = new TranslatorToMsil.TranslatorToMsil();
executor.Init(new ExecutorConfiguration(module));

var results = executor.RunModule();
Console.WriteLine(results.First());