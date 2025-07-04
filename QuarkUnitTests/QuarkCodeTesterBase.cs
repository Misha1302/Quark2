using QuarkExtension;
using QuarkStructures;

namespace UnitTests;

public class QuarkCodeTesterBase
{
    private readonly List<IQuarkExtension> _defaultExtensions =
        [new QuarkExtStructures(), new QuarkListInitializer.QuarkListInitializer()];

    protected void TestCode(string code, Action<Any> result, List<IQuarkExtension>? extensions = null)
    {
        var runner = new QuarkRunner.QuarkRunner();
        var resultInterpreter = runner.Execute(code, new QuarkVirtualMachine(), extensions ?? _defaultExtensions);
        var resultTranslator =
            runner.Execute(code, new TranslatorToMsil.TranslatorToMsil(), extensions ?? _defaultExtensions);

        result(resultTranslator);
        result(resultInterpreter);
    }
}