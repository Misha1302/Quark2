import "../../../../Libraries"

def Main() {
    // _ = PrintGreeting(5)
    PrintLn(IsPrime(122344388900113))

    return 0
}

def IsPrime(n) {
    top = n ** (1 / 2) + 1
    for (i = 2) (i <= top) (i = i + 1) {
        // PrintLn(i)
        if (n % i == 0) {
            return 0
        }
    } 
    
    return 1
}

def PrintGreeting() {
    PrintLn("This is the console project. Nothing interesting. You can see ToDo application in web api project")
    return 0
}