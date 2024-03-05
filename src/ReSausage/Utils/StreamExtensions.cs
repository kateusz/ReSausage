namespace ReSausage.Utils;

public static class StreamExtensions
{
    public static async Task<int> ReadNextInt32(this Stream stream)
    {
        var buffer = new byte[4];
        await stream.ReadAsync(buffer).ConfigureAwait(false);
        return BitConverter.ToInt32(buffer, 0);
    }

    public static async Task<int> ReadNextInt16(this Stream stream)
    {
        var buffer = new byte[2];
        await stream.ReadAsync(buffer).ConfigureAwait(false);
        return BitConverter.ToInt16(buffer, 0);
    }
}