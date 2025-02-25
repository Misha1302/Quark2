namespace QuarkBenchmarks;

// ReSharper disable once ClassNeverInstantiated.Global
public class IsPrimeTest() : QuarkTest(
    """
    import "../../../../../../../../Libraries"

    def Main() {
        n = 122344388900113
        isPrime = IsPrime(n)
        // _ = Print(isPrime)
        return 0
    }

    def IsPrime(n) {
        top = n ** (1 / 2) + 1
        for (i = 2) (i <= top) (i = i + 1) {
            if (n % i == 0) {
                return 0
            }
        } 
        
        return 1
    }
    """
);