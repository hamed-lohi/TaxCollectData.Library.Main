using FluentAssertions;
using TaxCollectData.Library.Abstraction;
using TaxCollectData.Library.Business;
using Xunit;

namespace TaxCollectData.Test;

public class PacketCodecTests
{
    private readonly IPacketCodec _packetCodec;

    public PacketCodecTests()
    {
        _packetCodec = new PacketCodec();
    }

    [Fact]
    public void XorTest()
    {
        var actual = _packetCodec.Xor(new byte[] { 0, 1, 0 }, new byte[] { 1, 0, 1 });
        actual.Should().Equal(1, 1, 1);
    }
}