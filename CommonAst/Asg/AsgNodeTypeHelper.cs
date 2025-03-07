namespace DefaultAstImpl.Asg;

public class AsgNodeTypeHelper
{
    private static long _num = (int)AsgNodeType.Removed;

    public static long GetNextFreeNumber() => ++_num;
}