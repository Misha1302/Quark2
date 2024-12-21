using AbstractExecutor;
using CommonBytecode.Data.AnyValue;
using QuarkCFrontend;
using QuarkCFrontend.Asg;
using QuarkCFrontend.Lexer;
using VirtualMachine;
using VirtualMachine.Vm.Data;
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

InitializeQuark();

app.Run();


void InitializeQuark()
{
    var code2 = File.ReadAllText("Code/Main.lua");

    var lexemes = new Lexer(LexerConfiguration.GetPatterns().ToList()).Lexemize(code2);
    var asg = new AsgBuilder(AsgBuilderConfiguration.Default).Build(lexemes);
    Console.WriteLine(asg);
    var module = new AsgToBytecodeTranslator.AsgToBytecodeTranslator().Translate(asg);
    var executor = (IExecutor)new QuarkVirtualMachine(new ExecutorConfiguration(GetBuildInFunctions()));

    // Console.WriteLine(module);
    executor.RunModule(module, [null]);
}

Dictionary<string, Action<Any>> GetBuildInFunctions()
{
    var functions = new Dictionary<string, Action<Any>>
    {
        ["AddGetEndpoint"] = rted =>
        {
            ExtractArgs(rted, out var interpreter, out var name);
            app.MapGet("Quark/" + name, () => Call([], interpreter, rted.Get<EngineRuntimeData>(), name))
                .WithOpenApi();
        },
        ["AddPostEndpoint"] = rted =>
        {
            ExtractArgs(rted, out var interpreter, out var name);
            app.MapPost("Quark/" + name,
                    (string str) => Call([VmValue.CreateRef(str, BytecodeValueType.Str)], interpreter,
                        rted.Get<EngineRuntimeData>(), name))
                .WithOpenApi();
        },
        ["AddDeleteEndpoint"] = rted =>
        {
            var interpreter = rted.Get<EngineRuntimeData>().CurInterpreter;
            var stack = interpreter.Stack;
            var needArgument = stack.Pop().IsTrue();
            var name = stack.Pop().GetRef<string>();
            stack.Pop();
            if (needArgument)
                app.MapDelete("Quark/" + name,
                        (string str) => Call([VmValue.CreateRef(str, BytecodeValueType.Str)], interpreter,
                            rted.Get<EngineRuntimeData>(), name))
                    .WithOpenApi();
            else
                app.MapDelete("Quark/" + name,
                        () => Call([], interpreter, rted.Get<EngineRuntimeData>(), name))
                    .WithOpenApi();
        },
        ["AddPutEndpoint"] = rted =>
        {
            ExtractArgs(rted, out var interpreter, out var name);
            app.MapPut("Quark/" + name,
                    (string str) => Call([VmValue.CreateRef(str, BytecodeValueType.Str)], interpreter,
                        rted.Get<EngineRuntimeData>(), name))
                .WithOpenApi();
        },
    };
    return functions;
}

string Call(Span<VmValue> args, Interpreter interpreter, EngineRuntimeData engineRuntimeData, string funcToCall)
{
    return interpreter.ExecuteFunction(funcToCall, args, engineRuntimeData).GetRef<string>();
}

void ExtractArgs(Any rted, out Interpreter interpreter, out string name)
{
    interpreter = rted.Get<EngineRuntimeData>().CurInterpreter;
    var stack = interpreter.Stack;
    name = stack.Pop().GetRef<string>();
    stack.Pop();
}