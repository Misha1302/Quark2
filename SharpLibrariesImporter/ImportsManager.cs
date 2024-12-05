using System.Reflection;
using ExceptionsManager;

namespace SharpLibrariesImporter;

public class ImportsManager
{
    private readonly Dictionary<MethodInfo, Delegate> _methods = new();

    public IReadOnlyDictionary<MethodInfo, Delegate> Methods => _methods;

    public Delegate GetDelegateByName(string name)
    {
        var first = _methods.FirstOrDefault(x => x.Key.Name == name);
        if (first.Value == null)
            Throw.InvalidOpEx($"Method name is invalid: {name}");
        return first.Value!;
    }

    public void Import(string path)
    {
        if (Directory.Exists(path)) ImportDirectory(path);
        else if (File.Exists(path)) ImportFile(path);
        else Throw.InvalidOpEx($"Unknown path \"{Path.GetFullPath(path)}\"");
    }

    private void ImportFile(string path)
    {
        if (!path.EndsWith(".dll")) Throw.InvalidOpEx("File is not a dll");

        var fullPath = Path.GetFullPath(path);
        var assembly = Assembly.LoadFile(fullPath);

        ImportAssembly(assembly);
    }

    private void ImportDirectory(string path)
    {
        foreach (var dir in Directory.GetDirectories(path))
            ImportDirectory(dir);

        foreach (var file in Directory.GetFiles(path))
            ImportFile(file);
    }


    private void ImportAssembly(Assembly assembly)
    {
        foreach (var type in assembly.GetTypes())
        foreach (var methodInfo in type.GetMethods(BindingFlags.Static | BindingFlags.Public))
            _methods.Add(methodInfo, methodInfo.CreateDelegateCustom(null));
    }
}