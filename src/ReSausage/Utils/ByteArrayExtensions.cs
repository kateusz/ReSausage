using SixLabors.ImageSharp.PixelFormats;

namespace ReSausage.Utils;

public static class ByteArrayExtensions
{
    public static Rgba32[,] ConvertToColorMatrix(this byte[] linearData, int width, int height, int bytesPerPixel)
    {
        var pixelArray = new byte[height, width * bytesPerPixel]; // 3 bytes per pixel

        var index = 0;
        for (var y = height - 1; y >= 0; y--)
        {
            for (var x = 0; x < width * bytesPerPixel; x += bytesPerPixel)
            {
                pixelArray[y, x] = linearData[index++]; // Red channel
                pixelArray[y, x + 1] = linearData[index++]; // Green channel
                pixelArray[y, x + 2] = linearData[index++]; // Blue channel
            }
        }

        var colorArray = new Rgba32[height, width];

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var r = pixelArray[y, x * 3]; // Red channel
                var g = pixelArray[y, x * 3 + 1]; // Green channel
                var b = pixelArray[y, x * 3 + 2]; // Blue channel

                colorArray[y, x] = new Rgba32(r, g, b);
            }
        }

        return colorArray;
    }
}