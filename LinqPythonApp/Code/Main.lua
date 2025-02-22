print(
    use x, i over a := [1] * 15 skip 5 skip 5 select { 
        global a, i
        a[i] = a[i - 1] + a[i - 2]
        return a[i] 
    } end
)