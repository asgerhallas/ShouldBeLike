using DeepEqual.Syntax;
using Newtonsoft.Json.Linq;
using Xunit;

namespace ShouldBeLike.Tests
{
    public class JTokenComparisonTests
    {
        public JTokenComparisonTests()
        {
            Extensions.DefaultTestingComparisonBuilder.WithCustomComparison(new JTokenComparison());
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
            });
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
                }));
        }
    }
}