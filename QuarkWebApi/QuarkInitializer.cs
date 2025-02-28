namespace QuarkWebApi;

public class QuarkInitializer
{
    public WebApplication InitializeQuark(WebApplicationBuilder builder, RunType runType)
    {
        var module = CreateBytecodeModule();
        var executor = CreateExecutor(runType, module);

        AddSingletons(builder, executor);

        var app = CreateApp(builder);

        QuarkEndpoints.Init(app);

        executor.RunModule();

        return app;
    }

    private static void AddSingletons(WebApplicationBuilder builder, IExecutor executor)
    {
        builder.Services.AddSingleton(executor);
    }

    private IExecutor CreateExecutor(RunType runType, BytecodeModule module)
    {
        var executor = (IExecutor)(runType != RunType.RunningUsingInterpreter
            ? new QuarkVirtualMachine()
            : new TranslatorToMsil.TranslatorToMsil());
        executor.Init(new ExecutorConfiguration(module));
        return executor;
    }

    private static BytecodeModule CreateBytecodeModule()
    {
        var code2 = File.ReadAllText("Code/Main.lua");

        var lexemes = new Lexer(QuarkLexerDefaultConfiguration.CreateDefault()).Lexemize(code2);
        var asg = new AsgBuilder<QuarkLexemeType>(QuarkAsgBuilderConfiguration.CreateDefault()).Build(lexemes);
        var module = new AsgToBytecodeTranslator<QuarkLexemeType>().Translate(asg);
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