using DeepEqual;
using DeepEqual.Syntax;
using Newtonsoft.Json.Linq;
using Xunit;

namespace ShouldBeLike.Tests
{
    public class JTokenComparisonTests
    {
        readonly CompositeComparison comparison;

        public JTokenComparisonTests()
        {
            comparison = Extensions.CreateTestingComparisonBuilder()
                .WithCustomComparison(new JTokenComparison())
                .Create();
        }

        [Fact]
        public void JObjectShouldBeLike()
        {
            new JObject
            {
                ["name"] = "Andreas"
            }.ShouldBeLike(new JObject
            {
                ["name"] = "Andreas"
            }, comparison);
        }

        [Fact]
        public void DifferentJObjectFails()
        {
            Assert.Throws<DeepEqualException>(() =>
                new JObject
                {
                    ["firstname"] = "Andreas"
                }.ShouldBeLike(new JObject
                {
                    ["lastname"] = "Andreas"
                }, comparison));
        }

    }
}