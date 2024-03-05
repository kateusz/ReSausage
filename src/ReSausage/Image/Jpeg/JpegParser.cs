using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ReSausage.Image.Jpeg;

internal sealed class JpegParser : IImageParser
{
    public async Task<Rgba32[,]> GetColorMatrix(string filePath)
    {
        using var image = await SixLabors.ImageSharp.Image.LoadAsync<Rgba32>(filePath).ConfigureAwait(false);

        var width = image.Width;
        var height = image.Height;

        var colors = ConvertTo2D(image, width, height);
        return colors;
    }

    private static Rgba32[,] ConvertTo2D(Image<Rgba32> image, int sourceWidth, int sourceHeight)
    {
        var pixels = new Rgba32[sourceWidth, sourceHeight];

        for (var y = 0; y < sourceHeight; y++)
        {
            for (var x = 0; x < sourceWidth; x++)
            {
                pixels[x, y] = image[x, y];
            }
        }

        return pixels;
    }
}