using AbstractExecutor;
using QuarkCFrontend;
using QuarkCFrontend.Asg;
using QuarkCFrontend.Lexer;
using VirtualMachine;

namespace QuarkWebApi;

public class QuarkInitializer
{
    public void InitializeQuark(IEndpointRouteBuilder app, RunType runType)
    {
        var code2 = File.ReadAllText("Code/Main.lua");

        var lexemes = new Lexer(LexerConfiguration.GetPatterns().ToList()).Lexemize(code2);
        var asg = new AsgBuilder(AsgBuilderConfiguration.Default).Build(lexemes);
        var module = new AsgToBytecodeTranslator.AsgToBytecodeTranslator().Translate(asg);
        var executor = (IExecutor)(
            runType != RunType.RunningUsingInterpreter
                ? new QuarkVirtualMachine(new ExecutorConfiguration())
                : new ToMsilTranslator.ToMsilTranslator()
        );

        QuarkEndpoints.Init(executor, module, app);

        executor.RunModule(module, [null]);
    }
}