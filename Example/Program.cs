using CommonLoggers;
using QuarkStructures;
using VirtualMachine;

const string code =
    """
    import "../../../../Libraries"

    def Main() {
        a:Number = 5
        _ = PrintLn(a)
        return 0
    }
    """;

// добавить авто-диспетчеризацию функций (Dictionary) по типам данных 

// var executor = new TranslatorToMsil.TranslatorToMsil();
var executor = new QuarkVirtualMachine();
var runner = new QuarkRunner.QuarkRunner();

var result = runner.Execute(code, executor,
    [
        new QuarkExtStructures(),
        new QuarkListInitializer.QuarkListInitializer(),
        new QuarkTypeSystemExt.QuarkTypeSystemExt(),
    ],
    new FileLogger("../../logs.txt")
);
Console.WriteLine(result);