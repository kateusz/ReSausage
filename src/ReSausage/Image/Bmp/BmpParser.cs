using System.Text;
using ReSausage.Utils;
using SixLabors.ImageSharp.PixelFormats;

namespace ReSausage.Image.Bmp;

internal sealed class BmpParser : IImageParser
{
    public async Task<Rgba32[,]> GetColorMatrix(string filePath)
    {
        var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        await using var _ = fileStream.ConfigureAwait(false);

        // HEADER 14 bytes
        // check file signature
        var signature = new byte[2];
        await fileStream.ReadAsync(signature).ConfigureAwait(false);
        var fileSignature = Encoding.ASCII.GetString(signature);

        // TODO: implement OSX etc.
        // https://en.wikipedia.org/wiki/BMP_file_format
        if (fileSignature != "BM")
        {
            throw new ImageLoadException("Invalid BMP signature.");
        }

        // Read file size
        var fileSize = await fileStream.ReadNextInt32().ConfigureAwait(false);

        // reserved
        // Move cursor - unused field
        fileStream.Seek(4, SeekOrigin.Current);

        // InfoHeader = 54 bytes = 40 + 14
        var dataOffset = await fileStream.ReadNextInt32().ConfigureAwait(false);
        if (dataOffset != 54)
        {
            throw new ImageLoadException("Invalid size of BMP header.");
        }

        // reserved
        fileStream.Seek(4, SeekOrigin.Current); // Move cursor - unused field

        // InfoHeader START 40 bytes
        var width = await fileStream.ReadNextInt32().ConfigureAwait(false);
        var height = await fileStream.ReadNextInt32().ConfigureAwait(false);
        var planes = await fileStream.ReadNextInt16().ConfigureAwait(false);

        /*
         * 	Bits per Pixel used to store palette entry information. This also identifies in an indirect way the number of possible colors. Possible values are:
           1 = monochrome palette. NumColors = 1
           4 = 4bit palletized. NumColors = 16
           8 = 8bit palletized. NumColors = 256
           16 = 16bit RGB. NumColors = 65536
           24 = 24bit RGB. NumColors = 16M
         */
        var bitsPerPixel = await fileStream.ReadNextInt16().ConfigureAwait(false);
        if (bitsPerPixel != 24 && bitsPerPixel != 32)
        {
            throw new ImageLoadException("Unsupported bits per pixel value.");
        }

        /*
         * Type of Compression
           0 = BI_RGB   no compression
           1 = BI_RLE8 8bit RLE encoding
           2 = BI_RLE4 4bit RLE encoding
         */
        var compression = await fileStream.ReadNextInt32().ConfigureAwait(false);

        // imageSize = 0 if compression = 0
        var imageSize = await fileStream.ReadNextInt32().ConfigureAwait(false);
        var xPixelsPerMeter = await fileStream.ReadNextInt32().ConfigureAwait(false);
        var yPixelsPerMeter = await fileStream.ReadNextInt32().ConfigureAwait(false);

        var colorsUsed = await fileStream.ReadNextInt32().ConfigureAwait(false);
        var importantColors = await fileStream.ReadNextInt32().ConfigureAwait(false);

        // read pixels
        var pixelData = new byte[fileSize - dataOffset - 4]; // file size - header size
        await fileStream.ReadAsync(pixelData).ConfigureAwait(false);

        var colors = pixelData.ConvertToColorMatrix(width, height, bitsPerPixel / 8);
        return colors;
    }
}