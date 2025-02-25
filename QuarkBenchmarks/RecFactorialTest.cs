namespace QuarkBenchmarks;

// ReSharper disable once ClassNeverInstantiated.Global
public class RecFactorialTest() : QuarkTest(
    """
    import "../../../../../../../../Libraries"

    def Main() {
        return Fact(1000)
    }

    def Fact(i) {
        if i <= 1 { return i }
        return Fact(i - 1) * i
    }
    """
);