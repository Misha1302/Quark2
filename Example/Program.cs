const string code =
    """
    import "../../../../Libraries"

    def Main() {
        a = CreateVector(5,6,2)
        
        _ = a.SetSize(5)
        _ = a.SetValue(2, 2)
        _ = a.SetValue(3, 6)
        _ = a.SetValue(4, 8)
        _ = PrintLn(a)
        
        _ = PrintLn(1.Sum(2).Sum(3).Sum(4).Sum(5))

        return 0
    }

    def Sum(a, b) { return a + b }
    """;

// var executor = new TranslatorToMsil.TranslatorToMsil();
var executor = new VirtualMachine.QuarkVirtualMachine();
var runner = new QuarkRunner.QuarkRunner();

var result = runner.Execute(code, executor, []);
Console.WriteLine(result);


// АУРУС - практика + низуровневое