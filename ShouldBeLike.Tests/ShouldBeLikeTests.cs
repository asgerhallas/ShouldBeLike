using DeepEqual;
using DeepEqual.Syntax;
using Xunit;

namespace ShouldBeLike.Tests
{
    public class ShouldBeLikeTests
    {
        [Fact]
        public void Object()
        {
            new object().ShouldBeLike(new object());
        }

        [Fact]
        public void OtherObject()
        {
            Assert.Throws<DeepEqualException>(() =>
                new object().ShouldBeLike(new Blah()));
        }

        public class Blah { }

        [Fact]
        public void ShouldFail()
        {
            Assert.Throws<DeepEqualException>(() =>
                ((object)new Foo(12)).ShouldBeLike(42));
        }

        [Fact]
        public void ConvertInconclusiveToFail_IfTypesAreDifferent()
        {
            // if types are the same it will throw and prompt to add a new Comparison to deal with this type.
            
            var (result, context) = new TestingComparisonBuilder().Create().Compare(new ComparisonContext(), new Foo(12), 42);

            Assert.Equal(ComparisonResult.Fail, result);
        }

        public class Foo
        {
            public int Value { get; }

            public Foo(int value) => Value = value;
        }
    }
}
