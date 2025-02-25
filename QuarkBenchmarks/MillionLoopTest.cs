namespace QuarkBenchmarks;

// ReSharper disable once ClassNeverInstantiated.Global
public class MillionLoopTest() : QuarkTest(
    """
    import "../../../../../../../../Libraries"

    def Main() {
        sum = 0
        for (i = 0) (i < 1000000) (i = i + 1) {
            sum = sum + i
        }
        return sum
    }
    """
);