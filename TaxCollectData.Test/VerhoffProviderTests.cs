using FluentAssertions;
using TaxCollectData.Library.Abstraction;
using TaxCollectData.Library.Business;
using Xunit;

namespace TaxCollectData.Test;

public class VerhoffProviderTests
{
    private const string NUMBER = "123";
    private readonly IVerhoffProvider _verhoffProvider;

    public VerhoffProviderTests()
    {
        _verhoffProvider = new VerhoffProvider();
    }

    [Fact]
    public void ValidateVerhoeffTest()
    {
        _verhoffProvider.ValidateVerhoeff(NUMBER).Should().BeFalse();
    }

    [Fact]
    public void GenerateVerhoeffTest()
    {
        _verhoffProvider.GenerateVerhoeff(NUMBER).Should().Be("3");
    }
}