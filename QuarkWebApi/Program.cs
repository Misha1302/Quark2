using QuarkWebApi;

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

// you can use:
// 1. RunningUsingMsilTranslator
// 2. RunningUsingInterpreter
new QuarkInitializer().InitializeQuark(app, RunType.RunningUsingMsilTranslator);

app.Run();