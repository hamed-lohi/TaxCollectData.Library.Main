using FluentAssertions;
using TaxCollectData.Library.Abstraction;
using TaxCollectData.Library.Business.Normalize;
using TaxCollectData.Library.Dto.Content;
using TaxCollectData.Library.Enums;
using Xunit;

namespace TaxCollectData.Test;

public class ObjectNormalizerTests
{
    private readonly INormalizer _normalizer;

    public ObjectNormalizerTests()
    {
        _normalizer = new ObjectNormalizer(new());
    }

    [Fact]
    public void NormalizeTest()
    {
        var filterDto = new FilterDto("Field", OperatorType.EQ, "Value");
        _normalizer.Normalize(filterDto, new()).Should().Be("Field#0#Value");
    }
}