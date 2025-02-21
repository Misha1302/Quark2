using AbstractExecutor;
using CommonBytecode.Data.Structures;
using DefaultAstImpl.Asg;
using DefaultLexerImpl.Lexer;
using QuarkCFrontend;
using VirtualMachine;

namespace QuarkWebApi;

public class QuarkInitializer
{
    public WebApplication InitializeQuark(WebApplicationBuilder builder, RunType runType)
    {
        var module = CreateBytecodeModule();
        var executor = CreateExecutor(runType);

        AddSingletons(builder, module, executor);

        var app = CreateApp(builder);

        QuarkEndpoints.Init(app);

        executor.RunModule(module);

        return app;
    }

    private static void AddSingletons(WebApplicationBuilder builder, BytecodeModule module, IExecutor executor)
    {
        builder.Services.AddSingleton(module);
        builder.Services.AddSingleton(executor);
    }

    private IExecutor CreateExecutor(RunType runType)
    {
        var executor = (IExecutor)(runType != RunType.RunningUsingInterpreter
            ? new QuarkVirtualMachine(new ExecutorConfiguration())
            : new ToMsilTranslator.ToMsilTranslator());
        return executor;
    }

    private static BytecodeModule CreateBytecodeModule()
    {
        var code2 = File.ReadAllText("Code/Main.lua");

        var lexemes = new Lexer(LexerDefaultConfiguration.CreateDefault()).Lexemize(code2);
        var asg = new AsgBuilder(AsgBuilderConfiguration.CreateDefault()).Build(lexemes);
        var module = new AsgToBytecodeTranslator.AsgToBytecodeTranslator().Translate(asg);
        return module;
    }

    private static WebApplication CreateApp(WebApplicationBuilder builder)
    {
        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();

        return app;
    }
}