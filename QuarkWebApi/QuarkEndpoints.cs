using AbstractExecutor;
using CommonBytecode.Data.AnyValue;
using CommonBytecode.Data.Structures;

namespace QuarkWebApi;

public static class QuarkEndpoints
{
    private static IExecutor _executor = null!;
    private static BytecodeModule _module = null!;
    private static IEndpointRouteBuilder _app = null!;

    public static void AddGetEndpoint(Any name)
    {
        _app
            .MapGet("Quark/" + name, () => _executor.RunFunction(_module, name.Get<string>(), []).First().Get<string>())
            .WithOpenApi();
    }

    public static void AddPostEndpoint(Any name)
    {
        _app
            .MapPost("Quark/" + name,
                (string text) =>
                    _executor.RunFunction(_module, name.Get<string>(), [text.ToAny()]).First().Get<string>())
            .WithOpenApi();
    }

    public static void AddDeleteEndpoint(Any name, Any needArgument)
    {
        if (needArgument.IsTrue())
            _app
                .MapDelete("Quark/" + name,
                    (string text) => _executor.RunFunction(_module, name.Get<string>(), [text.ToAny()]).First()
                        .Get<string>())
                .WithOpenApi();
        else
            _app
                .MapDelete("Quark/" + name,
                    () => _executor.RunFunction(_module, name.Get<string>(), []).First().Get<string>())
                .WithOpenApi();
    }

    public static void AddPutEndpoint(Any name)
    {
        _app
            .MapPut("Quark/" + name,
                (string text) =>
                    _executor.RunFunction(_module, name.Get<string>(), [text.ToAny()]).First().Get<string>())
            .WithOpenApi();
    }

    public static void Init(IExecutor executor, BytecodeModule module, IEndpointRouteBuilder app)
    {
        _executor = executor;
        _module = module;
        _app = app;
    }
}