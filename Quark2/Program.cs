using AbstractExecutor;
using Quark2;
using QuarkCFrontend;
using QuarkCFrontend.Asg;
using QuarkCFrontend.Lexer;
using VirtualMachine;

var code2 = File.ReadAllText("Code/Main");


var quarkStatistics = new QuarkStatistics();
var lexemes = quarkStatistics.Measure(() => new Lexer(LexerConfiguration.GetPatterns().ToList()).Lexemize(code2));
var asg = quarkStatistics.Measure(() => new AsgBuilder(AsgBuilderConfiguration.Default).Build(lexemes));
Console.WriteLine(asg);
var module = quarkStatistics.Measure(() => new AsgToBytecodeTranslator.AsgToBytecodeTranslator().Translate(asg));
var executor = (IExecutor)new QuarkVirtualMachine(new ExecutorConfiguration([]));
Console.WriteLine(module);
var results = quarkStatistics.Measure(() => executor.RunModule(module, [null]));

Console.WriteLine($"Results: {string.Join(", ", results)}");
Console.WriteLine("Statistics:");
Console.WriteLine(quarkStatistics.ToString());