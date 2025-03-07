using QuarkExtension;

namespace UnitTests;

public class CodeTesterBase
{
    protected void TestCode(string code, Action<Any> result, List<IQuarkExtension>? extensions = null)
    {
        var runner = new QuarkRunner.QuarkRunner();
        var resultInterpreter = runner.Execute(code, new QuarkVirtualMachine(), extensions ?? []);
        var resultTranslator = runner.Execute(code, new TranslatorToMsil.TranslatorToMsil(), extensions ?? []);

        result(resultTranslator);
        result(resultInterpreter);
    }
}