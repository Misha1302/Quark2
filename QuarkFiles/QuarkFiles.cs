using SharpAnyType;

namespace QuarkFiles;

public static class QuarkFiles
{
    public static void WriteText(Any path, Any text) => File.WriteAllText(path.Get<string>(), text.ToString());

    public static Any ReadText(Any path) => File.ReadAllText(path.Get<string>());
}