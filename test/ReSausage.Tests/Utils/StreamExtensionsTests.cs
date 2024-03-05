using ReSausage.Utils;

namespace ReSausage.Tests.Utils;

public class StreamExtensionsTests
{
    [Fact]
    public async Task ReadNextInt32_ShouldReturnCorrectValue()
    {
        var stream = new MemoryStream(BitConverter.GetBytes(1234));
        var result = await stream.ReadNextInt32().ConfigureAwait(false);
        Assert.Equal(1234, result);
    }

    [Fact]
    public async Task ReadNextInt16_ShouldReturnCorrectValue()
    {
        var stream = new MemoryStream(BitConverter.GetBytes((short)12));
        var result = await stream.ReadNextInt16().ConfigureAwait(false);
        Assert.Equal(12, result);
    }
}