namespace CommonLoggers;

public class FileLogger : ILogger
{
    private readonly string _logsFile;
    private readonly int _separatorLen;

    public FileLogger(string logsTxt, int separatorLen = 100)
    {
        _logsFile = logsTxt;
        _separatorLen = separatorLen;
        File.WriteAllText(_logsFile, string.Empty);
    }

    public void SetTheme(string theme, string end = "\n")
    {
        File.AppendAllText(_logsFile, $"{new string('-', _separatorLen)}\n\n{theme}:{end}");
    }

    public void Log(string message)
    {
        File.AppendAllText(_logsFile, $"{message}\n");
    }
}