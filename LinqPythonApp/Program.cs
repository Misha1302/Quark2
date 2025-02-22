if (args.Length == 0)
    args = ["LinqPythonApp"];

if (args.Length == 1)
    args = [args[0], "-input:Code/Main.lua", "-output:console"];

var argsDict = args.Skip(1).ToDictionary(x => x.Split(":")[0].Replace("-", ""), x => x.Split(":")[1]);

var linqPython = new LinqPythonLib.LinqPythonLib();
if (argsDict["output"] == "console")
    linqPython.Run(File.ReadAllText(argsDict["input"]), Console.WriteLine);
else Console.WriteLine("Unknown output");