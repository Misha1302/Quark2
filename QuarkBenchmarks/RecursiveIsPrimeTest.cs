namespace QuarkBenchmarks;

// ReSharper disable once ClassNeverInstantiated.Global
public class RecursiveIsPrimeTest() : QuarkTest(
    """
    import "../../../../../../../../Libraries"

    def Main() {
        n = 132749
        top = n ** (1 / 2) + 1
        isPrime = IsPrimeRec(n, top, 2)
        return isPrime
    }

    def IsPrimeRec(n, top, i) {
        if (i >= top) { return 1 }
        if (n % i == 0) { return 0 }
    
        return IsPrimeRec(n, top, i + 1)
    }
    """
);