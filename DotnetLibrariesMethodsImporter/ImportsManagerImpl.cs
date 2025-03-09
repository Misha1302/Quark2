namespace SharpLibrariesImporter;

public class ImportsManagerImpl
{
    private readonly AssemblyFixer _assemblyFixer;
    private readonly Dictionary<MethodInfo, Delegate> _methods = new();
    private readonly IMethodValidator _methodValidator;

    private string _lastUsedDirectoryPath = string.Empty;

    public ImportsManagerImpl(IMethodValidator methodValidator)
    {
        _methodValidator = methodValidator;
        _assemblyFixer = new AssemblyFixer();

        _assemblyFixer.Enable(() => _lastUsedDirectoryPath);
    }

    public IReadOnlyDictionary<MethodInfo, Delegate> Methods => _methods;

    ~ImportsManagerImpl()
    {
        _assemblyFixer.Disable();
    }


    public void Import(string path)
    {
        if (Directory.Exists(path))
        {
            _lastUsedDirectoryPath = path;
            ImportDirectory(path);
        }
        else if (File.Exists(path))
        {
            _lastUsedDirectoryPath = path[..path.LastIndexOf('.')];
            ImportFile(path);
        }
        else
        {
            Throw.InvalidOpEx($"Unknown path \"{Path.GetFullPath(path)}\"");
        }
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
}