using DeepEqual.Syntax;
using Xunit;

namespace ShouldBeLike.Tests
{
    public class StructualEquatableComparisonTests
    {
        [Fact]
        public void TupleShouldBeLike()
        {
            (1, "hello", 2m, new object()).ShouldBeLike((1, "hello", 2m, new object()));
        }

        [Fact]
        public void TupleFails()
        {
            Assert.Throws<DeepEqualException>(() =>
                ("hello", "world").ShouldBeLike(("Not hello", "world")));
        }
    }
}