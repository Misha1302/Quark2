using QuarkWebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// you can use:
// 1. RunningUsingMsilTranslator
// 2. RunningUsingInterpreter
var app = new QuarkInitializer().InitializeQuark(builder, RunType.RunningUsingMsilTranslator);

app.Run();