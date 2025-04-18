def main2():
    global a, b
    a = ["Tom", "Vasya", "Nick", "Ivan", "Anastasia", "Nick", "Miroslav", "Bogdan"]
    b = use x, i over a where len(x) <= 4 sort end
    c = use x, i over b first end
    
    print(b, c)

def main():
    global n
    n = int(input("Enter fib number index: "))
    
    if n <= 2: 
        print(1)
        return
    
    nth_fib_number = (
        use x, i over a := [1] * (n) skip 2 select {
            a[i] = a[i - 1] + a[i - 2]
            return a[i] 
        } last end
    )
        
    print(nth_fib_number)
    
main2()