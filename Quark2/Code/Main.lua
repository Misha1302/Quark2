import "../../../../Libraries"

def Main() {
    // 1 - prime, 0 - not prime
    n = 122344388900113
    
    Print(n)
    
    if (IsPrime(n)) {
        PrintLn(" is prime")
        return 0
    }
    
    PrintLn(" is not prime")

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