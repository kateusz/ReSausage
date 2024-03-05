using SixLabors.ImageSharp.PixelFormats;

namespace ReSausage.Image;

public interface IImageParser
{
    Task<Rgba32[,]> GetColorMatrix(string filePath);
}