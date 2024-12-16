using AbstractExecutor;
using QuarkCFrontend;
using QuarkCFrontend.Asg;
using QuarkCFrontend.Lexer;
using VirtualMachine;
using VirtualMachine.Vm.Data;
using VirtualMachine.Vm.Execution;
using VirtualMachine.Vm.Execution.Executors;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching",
};

app.MapGet("/weatherforecast", () => { })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

InitializeQuark();

app.Run();


void InitializeQuark()
{
    var code2 = File.ReadAllText("Code/Main");

    var lexemes = new Lexer(LexerConfiguration.GetPatterns().ToList()).Lexemize(code2);
    var asg = new AsgBuilder(AsgBuilderConfiguration.Default).Build(lexemes);
    // Console.WriteLine(asg);
    var module = new AsgToBytecodeTranslator.AsgToBytecodeTranslator().Translate(asg);
    var executor = (IExecutor)new QuarkVirtualMachine(GetBuildInFunctions());

    // Console.WriteLine(module);
    executor.RunModule(module, [null]);
}

Dictionary<string, Action<EngineRuntimeData>> GetBuildInFunctions()
{
    var functions = new Dictionary<string, Action<EngineRuntimeData>
    {
        ["AddGetEndpoint"] = (rted) =>
        {
            var interpreter = rted.CurInterpreter;
            var stack = interpreter.Stack;
            var name = stack.Pop().GetRef<string>();
            app.MapGet(name, () => { MakeBody(stack, interpreter, rted); })
                .WithName("GetWeatherForecast")
                .WithOpenApi();
        },
        ["AddPostEndpoint"] = (stack, rted, interpreter) =>
        {
            var name = stack.Pop().GetRef<string>();
            app.MapPost(name, () => { MakeBody(stack, interpreter, rted); })
                .WithName("GetWeatherForecast")
                .WithOpenApi();
        },
    };
    return functions;
}

void MakeBody(MyStack<VmValue> myStack, Interpreter arg3, EngineRuntimeData engineRuntimeData)
{
    var funcToCall = myStack.Pop().GetRef<string>();
    arg3.Frames.Push(
        new VmFuncFrame(engineRuntimeData.Module.Functions.First(x => x.Name == funcToCall))
    );
}