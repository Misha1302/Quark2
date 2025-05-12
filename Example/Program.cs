using QuarkStructures;
using VirtualMachine;

const string code =
    """
    import "../../../../Libraries"

    def Main() {
        return CreateVector({5,6})
    }
    """;

// var executor = new TranslatorToMsil.TranslatorToMsil();
var executor = new QuarkVirtualMachine();
var runner = new QuarkRunner.QuarkRunner();

var result = runner.Execute(code, executor,
    [new QuarkExtStructures(), new QuarkListInitializer.QuarkListInitializer()]
);
Console.WriteLine(result);