using AbstractExecutor;
using Quark2;
using QuarkCFrontend;
using QuarkCFrontend.Asg;
using QuarkCFrontend.Lexer;

// Translator faster than interpreter in approximately 9.5 times 
// new Measurer().Run(1);
// return;

var code2 = File.ReadAllText("Code/Main.lua");

var quarkStatistics = new QuarkStatistics();
var lexemes =
    quarkStatistics.Measure(() => new Lexer(LexerConfiguration.GetPatterns().ToList()).Lexemize(code2));
var asg = quarkStatistics.Measure(() => new AsgBuilder(AsgBuilderConfiguration.Default).Build(lexemes));
var module =
    quarkStatistics.Measure(() => new AsgToBytecodeTranslator.AsgToBytecodeTranslator().Translate(asg));
var executor = new ToMsilTranslator.ToMsilTranslator(new ExecutorConfiguration([]));
var results = quarkStatistics.Measure(() => executor.RunModule(module, [null]));

Console.WriteLine($"Results: {string.Join(", ", results)}");
Console.WriteLine("Statistics:");
Console.WriteLine(quarkStatistics.ToString());