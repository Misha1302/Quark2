using AbstractExecutor;
using QuarkCFrontend;
using QuarkCFrontend.Asg;
using QuarkCFrontend.Lexer;
using QuarkWebApi;
using VirtualMachine;

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
return;


void InitializeQuark()
{
    var code2 = File.ReadAllText("Code/Main.lua");

    var lexemes = new Lexer(LexerConfiguration.GetPatterns().ToList()).Lexemize(code2);
    var asg = new AsgBuilder(AsgBuilderConfiguration.Default).Build(lexemes);
    var module = new AsgToBytecodeTranslator.AsgToBytecodeTranslator().Translate(asg);
    var executor = (IExecutor)new ToMsilTranslator.ToMsilTranslator(new ExecutorConfiguration());
    // var executor = (IExecutor)new QuarkVirtualMachine(new ExecutorConfiguration());

    QuarkEndpoints.Init(executor, module, app);

    executor.RunModule(module, [null]);
}