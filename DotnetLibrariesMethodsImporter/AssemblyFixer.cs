namespace SharpLibrariesImporter;

public class AssemblyFixer
{
    private Func<string> _lastImportedDirectoryGetter = null!;

    public void Enable(Func<string> lastImportedDirectoryGetter)
    {
        _lastImportedDirectoryGetter = lastImportedDirectoryGetter;
        AppDomain.CurrentDomain.AssemblyResolve += OnCurrentDomainOnAssemblyResolve;
    }

    private Assembly OnCurrentDomainOnAssemblyResolve(object? appDomain, ResolveEventArgs args)
    {
        var assembly = GetLoadedAssemblies((AppDomain)appDomain!).FirstOrDefault(x => x.FullName == args.Name);
        if (assembly != null) return assembly;

        var dllName = args.Name.Split(",")[0] + ".dll";
        var lastImportedDirectory = _lastImportedDirectoryGetter();
        Throw.AssertAlways(Directory.Exists(lastImportedDirectory));

        var foundDlls = Directory.GetFileSystemEntries(
            lastImportedDirectory,
            dllName,
            SearchOption.AllDirectories
        );

        return foundDlls.Length != 0
            ? Assembly.LoadFile(foundDlls[0])
            : Throw.InvalidOpEx<Assembly>($"Unable to load {args.Name}");
    }

    private Assembly[] GetLoadedAssemblies(AppDomain appDomain) => appDomain.GetAssemblies();

    public void Disable()
    {
        AppDomain.CurrentDomain.AssemblyResolve -= OnCurrentDomainOnAssemblyResolve;
    }

    ~AssemblyFixer()
    {
        AppDomain.CurrentDomain.AssemblyResolve += OnCurrentDomainOnAssemblyResolve;
    }
}