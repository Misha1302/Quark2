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

    public void Log(string theme, string message)
    {
        File.AppendAllText(_logsFile, $"{theme}: \n{message}\n\n{new string('-', _separatorLen)}\n\n");
    }
}