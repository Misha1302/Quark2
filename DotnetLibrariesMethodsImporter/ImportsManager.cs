namespace SharpLibrariesImporter;

public class ImportsManager(IMethodValidator methodValidator)
{
    private readonly ImportsManagerImpl _importsManager = new(methodValidator);

    public bool Have(string name)
    {
        return _importsManager.Methods.Any(x => x.Key.Name == name);
    }

    public Delegate GetDelegateByName(string name)
    {
        var first = _importsManager.Methods.FirstOrDefault(x => x.Key.Name == name);
        if (first.Value == null)
            Throw.InvalidOpEx($"Method name is invalid: {name}");
        return first.Value;
    }

    public void Import(string path) => _importsManager.Import(path);
}