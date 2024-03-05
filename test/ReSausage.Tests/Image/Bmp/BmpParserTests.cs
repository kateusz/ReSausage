using FluentAssertions;
using ReSausage.Image;
using ReSausage.Image.Bmp;

namespace ReSausage.Tests.Image.Bmp;

public class BmpParserTests
{
    [Fact]
    public async Task Load_ShouldNotThrowError()
    {
        var parser = new BmpParser();
        var action = async () => await parser.GetColorMatrix("assets/test.bmp").ConfigureAwait(false);
        await action.Should().NotThrowAsync<ImageLoadException>().ConfigureAwait(false);
    }
}