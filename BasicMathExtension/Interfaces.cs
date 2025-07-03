namespace BasicMathExtension;

public interface IAddable<TSelf> where TSelf : IAddable<TSelf>
{
    public static abstract TSelf Add(TSelf a, TSelf b);
}

public interface ISubable<TSelf> where TSelf : ISubable<TSelf>
{
    public static abstract TSelf Sub(TSelf a, TSelf b);
}

public interface IMulable<TSelf> where TSelf : IMulable<TSelf>
{
    public static abstract TSelf Mul(TSelf a, TSelf b);
}

public interface IDivable<TSelf> where TSelf : IDivable<TSelf>
{
    public static abstract TSelf Div(TSelf a, TSelf b);
}

public interface IModable<TSelf> where TSelf : IModable<TSelf>
{
    public static abstract TSelf Mod(TSelf a, TSelf b);
}

public interface IArithmeticOperable<TSelf>
    : IAddable<TSelf>, ISubable<TSelf>, IMulable<TSelf>, IDivable<TSelf>
    where TSelf : IAddable<TSelf>, ISubable<TSelf>, IMulable<TSelf>, IDivable<TSelf>;