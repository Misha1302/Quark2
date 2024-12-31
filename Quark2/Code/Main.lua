import "../../../../Libraries"

def Main() {
    _ = IsPrime(122344388900113)

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