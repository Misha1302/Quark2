using CommonLoggers;
using QuarkStructures;
using VirtualMachine;

const string code =
    """
    import "../../../../Libraries"

    def Main() {
        _ = PrintLn(Fact(5))
        return 0
    }
    
    def Fact(i) {
        if i <= 1 { return i }
        return Fact(i - 1) * i
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