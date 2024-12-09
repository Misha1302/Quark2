using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Quark2;

public class QuarkStatistics
{
    private readonly List<(long, string)> _times = [];

    public T Measure<T>(Func<T> func, [CallerArgumentExpression(nameof(func))] string expression = null!)
    {
        var sw = Stopwatch.StartNew();
        var result = func();
        _times.Add((sw.ElapsedMilliseconds, expression));
        return result;
    }

    public override string ToString() =>
        string.Join("\n", _times.Select(x => $"{x.Item1}".PadRight(6) + $": {x.Item2}"));
}