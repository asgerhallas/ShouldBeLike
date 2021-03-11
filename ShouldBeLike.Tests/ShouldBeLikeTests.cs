using System;
using System.Linq;
using System.Threading.Tasks;
using DeepEqual;
using DeepEqual.Syntax;
using Xunit;
using Xunit.Abstractions;

namespace ShouldBeLike.Tests
{
    public class ShouldBeLikeTests
    {
        readonly ITestOutputHelper output;

        public ShouldBeLikeTests(ITestOutputHelper output)
        {
            this.output = output;
        }

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

        [Fact]
        public void EmptyObject()
        {
            new Blah().ShouldBeLike(new Blah());
        }

        [Fact]
        public void EmptyObject_IgnoredProperty()
        {
            var comparison = Extensions.CreateTestingComparisonBuilder().IgnoreProperty<Blah2>(x => x.IgnoreMe).Create();

            new Blah2().ShouldBeLike(new Blah2(), comparison);
        }

        public class Blah { }
        public class Blah2 
        {
            public string IgnoreMe { get; set; }
        }

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

        [Fact]
        public void Bug_SetupToFail()
        {
            // A bug in the cycles detector: one failing assertions made other assertions false positive.
            // This could be seen in concurrent test runs, as it was also not thread safe

            var abc = new[] { "a", "b", "c" };
            try
            {
                abc.ShouldBeLike("b", "a", "c");
            }
            catch (Exception e)
            {
            }

            Assert.Throws<DeepEqualException>(() => abc.ShouldBeLike("b", "a", "c"));
        }

        public class Foo
        {
            public int Value { get; }

            public Foo(int value) => Value = value;
        }
    }
}
