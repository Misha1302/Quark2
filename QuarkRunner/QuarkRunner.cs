using AbstractExecutor;
using AsgToBytecodeTranslator;
using CommonBytecode.Data.Structures;
using CommonFrontendApi;
using DefaultAstImpl.Asg;
using DefaultLexerImpl;
using QuarkCFrontend;
using QuarkExtension;
using SharpAnyType;

namespace QuarkRunner;

public class QuarkRunner
{
    public Any Execute(string code, IExecutor executor, List<IQuarkExtension> extensions)
    {
        var lexemes = Lexemize(code, extensions);
        var asg = Parse(extensions, lexemes);
        var module = Translate(extensions, asg);
        InitExecutor(executor, module);
        var result = Execute(executor);
        return result;
    }

    public Any Execute(IExecutor executor) => executor.RunModule().First();

    public void InitExecutor(IExecutor executor, BytecodeModule module)
    {
        executor.Init(new ExecutorConfiguration(module));
    }

    public BytecodeModule Translate(List<IQuarkExtension> extensions, AsgNode<QuarkLexemeType> asg)
    {
        var translatorHandlers = extensions.Aggregate((Action<AsgToBytecodeData<QuarkLexemeType>>)null!,
            (current, ext) => current + ext.GetUnknownAsgCodeHandler());
        var module = new AsgToBytecodeTranslator<QuarkLexemeType>().Translate(asg, translatorHandlers);
        Console.WriteLine(module);
        return module;
    }

    public AsgNode<QuarkLexemeType> Parse(List<IQuarkExtension> extensions,
        List<LexemeValue<QuarkLexemeType>> lexemes)
    {
        var parserConf = QuarkAsgBuilderConfiguration.CreateDefault();
        parserConf = extensions.Aggregate(parserConf, (current, ext) => ext.ExtendAsgBuilderConfiguration(current));
        var asg = new AsgBuilder<QuarkLexemeType>(parserConf).Build(lexemes);
        Console.WriteLine(asg);
        return asg;
    }

    public List<LexemeValue<QuarkLexemeType>> Lexemize(string code, List<IQuarkExtension> extensions)
    {
        var lexerConf = QuarkLexerDefaultConfiguration.CreateDefault();
        lexerConf = extensions.Aggregate(lexerConf, (current, ext) => ext.ExtendLexerConfiguration(current));
        var lexemes = new QuarkLexer(lexerConf).Lexemize(code);
        return lexemes;
    }
}