const string code =
    """
    import "../../../../Libraries"

    def Main() {
        img = CreateImage(256, 256)
        _ = SetPixel(img, 100, 100, CreateColor(255, 0, 0, 120))
        _ = SaveImageAsPng(img, "Pixel.png")
        _ = PrintLn("Saved!")
        return 0
    }
    """;

var executor = new TranslatorToMsil.TranslatorToMsil();
var runner = new QuarkRunner.QuarkRunner();

var result = runner.Execute(code, executor, []);
Console.WriteLine(result);