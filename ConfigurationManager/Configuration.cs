namespace ConfigurationManager;

public static class Configuration
{
    public static bool IsDebug
    {
        get
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }
    }
}