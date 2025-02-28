﻿using AbstractExecutor;
using AsgToBytecodeTranslator;
using DefaultAstImpl.Asg;
using DefaultLexerImpl;
using QuarkCFrontend;

const string code =
    """
    import "../../../../Libraries"

    def Main() {
        _ = PrintLn("Hello World!")
        return 0
    }
    """;

var lexemes = new Lexer(QuarkLexerDefaultConfiguration.CreateDefault()).Lexemize(code);
var asg = new AsgBuilder<QuarkLexemeType>(QuarkAsgBuilderConfiguration.CreateDefault()).Build(lexemes);
var module = new AsgToBytecodeTranslator<QuarkLexemeType>().Translate(asg);
var executor = new TranslatorToMsil.TranslatorToMsil();
executor.Init(new ExecutorConfiguration(module));
var results = executor.RunModule();
Console.WriteLine(results.First());