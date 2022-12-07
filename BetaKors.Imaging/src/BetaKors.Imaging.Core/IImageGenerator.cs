using SixLabors.ImageSharp;
using Rgba32 = SixLabors.ImageSharp.PixelFormats.Rgba32;

namespace BetaKors.Imaging.Core;

public interface IImageGenerator
{
    public Image<Rgba32> Generate();
}
