using QuarkExtension;
using QuarkStructures;

const string code =
    """
    import "../../../../Libraries"

    struct Vector3(x, y, z)

    def Main() {
        v = CreateStruct("Vector3")
        v2 = CreateStruct("Vector3")
        v->x = 3
        v2->y = 4
        _ = PrintLn(v)
        _ = PrintLn(v2)
        return 0
    }
    """;

var extensions = (List<IQuarkExtension>) [new QuarkExtStructures()];
var executor = new TranslatorToMsil.TranslatorToMsil();
var runner = new QuarkRunner.QuarkRunner();

var result = runner.Execute(code, executor, extensions);
Console.WriteLine(result);