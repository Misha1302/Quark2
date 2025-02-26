using System.Numerics;
using DynamicStrongTypeValue;
using SharpAnyType;
using StrongAnyValueCalculator;

var a = new Complex(2, 4);
var b = new Complex(4, 5);
Console.WriteLine(
    AnyOptCalculator.Sum(
        AnyOpt.CreateRef(a, AnyValueType.SomeSharpObject),
        AnyOpt.CreateRef(b, AnyValueType.SomeSharpObject)
    )
);

// using AbstractExecutor;
// using AsgToBytecodeTranslator;
// using DefaultAstImpl.Asg;
// using DefaultLexerImpl;
// using QuarkCFrontend;
// using VirtualMachine;
//
// const string code =
//     """
//     import "../../../../Libraries"
//
//     def Main() {
//         PrintLn("Hello World!")
//         return 0
//     }
//     """;
//
// var lexemes = new Lexer(QuarkLexerDefaultConfiguration.CreateDefault()).Lexemize(code);
// var asg = new AsgBuilder<QuarkLexemeType>(QuarkAsgBuilderConfiguration.CreateDefault()).Build(lexemes);
// var module = new AsgToBytecodeTranslator<QuarkLexemeType>().Translate(asg);
// var executor = new QuarkVirtualMachine();
// executor.Init(new ExecutorConfiguration(module));
// executor.RunModule();