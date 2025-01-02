using AbstractExecutor;
using CommonBytecode.Data.AnyValue;
using CommonBytecode.Data.Structures;
using Microsoft.AspNetCore.Mvc;

namespace QuarkWebApi;

public static class QuarkEndpoints
{
    private static IEndpointRouteBuilder _app = null!;

    public static void AddGetEndpoint(Any name)
    {
        _app
            .MapGet("Quark/" + name, (
                    [FromServices] IExecutor executor,
                    [FromServices] BytecodeModule module
                ) =>
                executor.RunFunction(module, name.Get<string>(), []).First().Get<string>());
    }

    public static void AddPostEndpoint(Any name)
    {
        _app
            .MapPost("Quark/" + name, (
                    string text,
                    [FromServices] IExecutor executor,
                    [FromServices] BytecodeModule module
                ) =>
                executor.RunFunction(module, name.Get<string>(), [text.ToAny()]).First().Get<string>());
    }

    public static void AddDeleteEndpoint(Any name, Any needArgument)
    {
        if (needArgument.IsTrue())
            _app
                .MapDelete("Quark/" + name, (
                        string text,
                        [FromServices] IExecutor executor,
                        [FromServices] BytecodeModule module
                    ) =>
                    executor.RunFunction(module, name.Get<string>(), [text.ToAny()]).First()
                        .Get<string>());

        else
            _app
                .MapDelete("Quark/" + name,
                    (
                        [FromServices] IExecutor executor,
                        [FromServices] BytecodeModule module
                    ) => executor.RunFunction(module, name.Get<string>(), []).First().Get<string>());
    }

    public static void AddPutEndpoint(Any name)
    {
        _app
            .MapPut("Quark/" + name, (
                    string text,
                    [FromServices] IExecutor executor,
                    [FromServices] BytecodeModule module
                ) =>
                executor.RunFunction(module, name.Get<string>(), [text.ToAny()]).First().Get<string>());
    }

    public static void Init(IEndpointRouteBuilder app)
    {
        _app = app;
    }
}