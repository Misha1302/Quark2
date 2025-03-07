const string code =
    """
    import "../../../../Libraries"

    def Main() {
        _ = PrintLn("Hello, World!")
        return 0
    }
    """;

var executor = new TranslatorToMsil.TranslatorToMsil();
var runner = new QuarkRunner.QuarkRunner();

var result = runner.Execute(code, executor, []);
Console.WriteLine(result);