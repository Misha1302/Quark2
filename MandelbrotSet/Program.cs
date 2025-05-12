const string code =
    """
    import "../../../../Libraries"

    def Main() {
        true = 1
        false = 0

        factor = 1 / 16
        P = 2048 * factor
        img = CreateImage(P * 2, P * 2)
        
        scale = P / 1.5
        n_iter = 100
        
        view0 = -900 * factor
        view1 = 0
        
        for (y = 0 - P + view1) (y < P + view1) (y = y + 1) {
            for (x = 0 - P + view0) (x < P + view0) (x = x + 1) {
               a = x / scale
               b = y / scale
               c = CreateComplex(a, b)
               z = CreateComplex(0, 0)
               n = 0
               stop = false
               for (n = 0) ((n < n_iter) and (not stop)) (n = n + 1) {
                   z = MulComplex(z, z) + c
                   if AbsComplex(z) > 2 {
                       stop = true
                   }
               }
               
               if (n >= n_iter - 1) {
                   r = GetRandomInteger(0, 35)
                   g = GetRandomInteger(0, 35)
                   b = GetRandomInteger(0, 35)
               }
               else {
                   r = (n % 2) * 32 + 127
                   g = (n % 4) * 64
                   b = (n % 2) * 16 + 128
               }
               
               _ = SetPixel(img, x + P - view0, y + P - view1, CreateColor(r, g, b, 255))
            }
        }
        
        _ = SaveImageAsPng(img, "Pixel.png")
        _ = PrintLn("Saved!")
        return 0
    }
    """;

var executor = new TranslatorToMsil.TranslatorToMsil();
var runner = new QuarkRunner.QuarkRunner();

var result = runner.Execute(code, executor, []);
Console.WriteLine(result);