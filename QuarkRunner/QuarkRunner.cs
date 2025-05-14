using AbstractExecutor;
using AsgToBytecodeTranslator;
using CommonBytecode.Data.Structures;
using CommonFrontendApi;
using CommonLoggers;
using DefaultAstImpl.Asg;
using DefaultLexerImpl;
using QuarkCFrontend;
using QuarkExtension;
using SharpAnyType;

namespace QuarkRunner;

public class QuarkRunner
{
    public Any Execute(string code, IExecutor executor, List<IQuarkExtension> extensions, ILogger? logger = null)
    {
        logger ??= new PlugLogger();
        logger.Log("Code", code);

        var lexemes = Lexemize(code, extensions, logger);
        var asg = Parse(extensions, lexemes, logger);
        // TODO: add asg validator
        var module = Translate(extensions, asg, logger);
        InitExecutor(executor, module);
        var result = Execute(executor);
        return result;
    }

    public Any Execute(IExecutor executor) => executor.RunModule().First();

    public void InitExecutor(IExecutor executor, BytecodeModule module)
    {
        executor.Init(new ExecutorConfiguration(module));
    }

    public BytecodeModule Translate(List<IQuarkExtension> extensions, AsgNode<QuarkLexemeType> asg, ILogger logger)
    {
        var translatorHandlers = extensions.Aggregate((Action<AsgToBytecodeData<QuarkLexemeType>>)null!,
            (current, ext) => current + ext.GetUnknownAsgCodeHandler());
        var module = new AsgToBytecodeTranslator<QuarkLexemeType>().Translate(asg, translatorHandlers);
        logger.Log(nameof(Translate), module.ToString());
        return module;
    }

    public AsgNode<QuarkLexemeType> Parse(List<IQuarkExtension> extensions,
        List<LexemeValue<QuarkLexemeType>> lexemes, ILogger logger)
    {
        var parserConf = QuarkAsgBuilderConfiguration.CreateDefault();
        parserConf = extensions.Aggregate(parserConf, (current, ext) => ext.ExtendAsgBuilderConfiguration(current));
        var asg = new AsgBuilder<QuarkLexemeType>(parserConf).Build(lexemes);
        logger.Log(nameof(Parse), asg.ToString());
        return asg;
    }

    public List<LexemeValue<QuarkLexemeType>> Lexemize(string code, List<IQuarkExtension> extensions, ILogger logger)
    {
        var lexerConf = QuarkLexerDefaultConfiguration.CreateDefault();
        lexerConf = extensions.Aggregate(lexerConf, (current, ext) => ext.ExtendLexerConfiguration(current));
        var lexemes = new QuarkLexer(lexerConf).Lexemize(code);
        logger.Log(nameof(Lexemize), string.Join("\n", lexemes));
        return lexemes;
    }
}