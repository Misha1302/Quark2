namespace TranslatorToMsil;

public static class DelegatesHelper
{
    public static MethodInfo GetInfo(this Delegate del) => del.Method;
}