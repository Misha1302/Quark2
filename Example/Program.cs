using QuarkStructures;
using VirtualMachine;

const string code =
    """
    import "../../../../Libraries"

    def Main() {
        _ = PrintLn("Hello, World!")
        return 0
    }
    """;

// var executor = new TranslatorToMsil.TranslatorToMsil();
var executor = new QuarkVirtualMachine();
var runner = new QuarkRunner.QuarkRunner();

var result = runner.Execute(code, executor,
    [new QuarkExtStructures(), new QuarkListInitializer.QuarkListInitializer()]
);
Console.WriteLine(result);