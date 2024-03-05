using FluentAssertions;
using ReSausage.Image;
using ReSausage.Image.Jpeg;

namespace ReSausage.Tests.Image.Jpeg;

public class JpegParserTests
{
    [Fact]
    public async Task Load_ShouldNotThrowError()
    {
        var parser = new JpegParser();
        var action = async () => await parser.GetColorMatrix("assets/test.jpg").ConfigureAwait(false);
        await action.Should().NotThrowAsync<ImageLoadException>().ConfigureAwait(false);
    }
}