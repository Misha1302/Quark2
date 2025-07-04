namespace CommonLoggers;

public interface ILogger
{
    public void SetTheme(string theme, string end = "\n");

    public void Log(string message);

    public void Log(string theme, string message)
    {
        SetTheme(theme);
        Log(message);
    }
}