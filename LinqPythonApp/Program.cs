if (args.Length == 0)
    args = ["-input:Code/Main.lua", "-output:console"];

var argsDict = args.ToDictionary(x => x.Split(":")[0].Replace("-", ""), x => x.Split(":")[1]);

var linqPython = new LinqPythonLib.LinqPythonLib();
var code = File.ReadAllText(argsDict["input"]);
var output = argsDict["output"];

if (output == "console")
    linqPython.Run(code, Console.WriteLine);
else if (output.Contains('.'))
    linqPython.Run(code, s => File.WriteAllText(output, s));
// go around the folder recursively and find all files with the extension .py
else Console.WriteLine("Unknown output");