using Doubles;
using SharpAnyType;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using ImgRgba32 = SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgba32>;

namespace QuarkGraphics;

public class QuarkGraphics
{
    public static Any CreateImage(Any width, Any height) =>
        new(
            new ImgRgba32(width.Get<double>().ToInt(), height.Get<double>().ToInt()),
            AnyValueType.SomeSharpObject
        );

    public static void SetPixel(Any img, Any x, Any y, Any color)
    {
        img.Get<ImgRgba32>()[x.Get<double>().ToInt(), y.Get<double>().ToInt()] = color.Get<Rgba32>();
    }

    public static void SaveImageAsPng(Any img, Any path)
    {
        img.Get<ImgRgba32>().SaveAsPng(path.Get<string>());
    }

    public static Any CreateColor(Any r, Any g, Any b, Any alpha) =>
        new(
            new Rgba32(
                r.Get<double>().ToInt(),
                g.Get<double>().ToInt(),
                b.Get<double>().ToInt(),
                alpha.Get<double>().ToInt()
            ),
            AnyValueType.SomeSharpObject
        );
}