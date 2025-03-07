using QuarkExtension;
using QuarkStructures;

const string code =
    """
    import "../../../../Libraries"

    struct Vector3(x, y, z)
    
    def Main() {
        v = CreateStruct("Vector3")
        v2 = CreateStruct("Vector3")
        v->x = v2
        v->x->y = 5
        v->y = 3
        _ = PrintLn(v)
        return v->x->y * v->y
    }
    """;

var extensions = (List<IQuarkExtension>) [new QuarkExtStructures()];
var executor = new TranslatorToMsil.TranslatorToMsil();
var runner = new QuarkRunner.QuarkRunner();

var result = runner.Execute(code, executor, extensions);
Console.WriteLine(result);