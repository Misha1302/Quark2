namespace SharpLibrariesImporter;

public class ImportsManager
{
    private readonly Dictionary<MethodInfo, Delegate> _methods = new();
    private readonly IMethodValidator _methodValidator;

    public ImportsManager(IMethodValidator methodValidator)
    {
        _methodValidator = methodValidator;

        AppDomain.CurrentDomain.AssemblyResolve += OnCurrentDomainOnAssemblyResolve;
    }

    public IReadOnlyDictionary<MethodInfo, Delegate> Methods => _methods;

    ~ImportsManager()
    {
        AppDomain.CurrentDomain.AssemblyResolve -= OnCurrentDomainOnAssemblyResolve;
    }

    private Assembly? OnCurrentDomainOnAssemblyResolve(object? sender, ResolveEventArgs args)
    {
        var assembly = ((AppDomain)sender!).GetAssemblies().FirstOrDefault(x => x.FullName == args.Name);
        return assembly;
    }

    public Delegate GetDelegateByName(string name)
    {
        var first = _methods.FirstOrDefault(x => x.Key.Name == name);
        if (first.Value == null)
            Throw.InvalidOpEx($"Method name is invalid: {name}");
        return first.Value;
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
        var assembly = Assembly.LoadFrom(fullPath);

        RawLoadAssembly(assembly);
    }

    private void ImportDirectory(string path)
    {
        foreach (var dir in Directory.GetDirectories(path))
            ImportDirectory(dir);

        foreach (var file in Directory.GetFiles(path).Where(IsCorrectFilePath))
            ImportFile(file);
    }

    private bool IsCorrectFilePath(string path) => path.EndsWith(".dll");

    private void RawLoadAssembly(Assembly assembly)
    {
        foreach (var type in assembly.GetTypes())
        foreach (var methodInfo in SelectMethods(type))
            _methods.TryAdd(methodInfo, methodInfo.CreateDelegateCustom(null));
    }

    private IEnumerable<MethodInfo> SelectMethods(Type type) =>
        type
            .GetMethods(~BindingFlags.Default)
            .Where(_methodValidator.IsValidMethod);


    public bool Have(string functionName)
    {
        return _methods.Any(x => x.Key.Name == functionName);
    }
}