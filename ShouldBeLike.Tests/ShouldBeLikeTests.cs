using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DeepEqual;
using DeepEqual.Syntax;
using Xunit;
using Xunit.Abstractions;

namespace ShouldBeLike.Tests
{
    public class ShouldBeLikeTests(ITestOutputHelper output)
    {
        readonly ITestOutputHelper output = output;

        [Fact]
        public void Object() => new object().ShouldBeLike(new object());

        [Fact]
        public void OtherObject() =>
            Assert.Throws<DeepEqualException>(() =>
                new object().ShouldBeLike(new Blah()));

        [Fact]
        public void ComparingObjectsWithNoProperties_DifferentType_DefaultBehavior()
        {
            var comparer = new ComparisonBuilder().Create();
            var context = new ComparisonContext();

            Assert.Equal(
                ComparisonResult.Pass, 
                comparer.Compare(context, new object(), new Blah()).result);
        }

        [Fact]
        public void EmptyObject() => new Blah().ShouldBeLike(new Blah());

        [Fact]
        public void EmptyObject_IgnoredProperty()
        {
            var comparison = ShouldBeLikeExtensions.CreateTestingComparisonBuilder().IgnoreProperty<Blah2>(x => x.IgnoreMe).Create();

            new Blah2().ShouldBeLike(new Blah2(), comparison);
        }

        [Fact]
        public void SameProperties_DifferentTypes() =>
            Assert.Throws<DeepEqualException>(() =>
                new Blah2().ShouldBeLike<object>(new Blah3()));

        [Fact]
        public void SameProperties_DifferentTypes_Records() =>
            Assert.Throws<DeepEqualException>(() =>
                new BlahRecord2(new List<int>() { 1 }).ShouldBeLike<object>(new BlahRecord3(new List<int>() { 1 })));

        [Fact]
        public void SameProperties_DifferentTypes_Lists() => 
            new BlahRecord2(new List<int> { 1 }).ShouldBeLike<object>(new BlahRecord2(new Collection<int> { 1 }));

        [Fact]
        public void SameProperties_DifferentTypes_Arrays() => 
            new BlahRecord2(new[] { 1 }).ShouldBeLike<object>(new BlahRecord2(new List<int> { 1 }));

        public class Blah { }
        public class Blah2 
        {
            public string IgnoreMe { get; set; }
        }
        public class Blah3
        {
            public string IgnoreMe { get; set; }
        }

        public record BlahRecord2(IReadOnlyList<int> ints);
        public record BlahRecord3(IReadOnlyList<int> ints);

        [Fact]
        public void ShouldFail() =>
            Assert.Throws<DeepEqualException>(() =>
                ((object)new Foo(12)).ShouldBeLike(42));

        [Fact]
        public void ConvertInconclusiveToFail_IfTypesAreDifferent()
        {
            // if types are the same it will throw and prompt to add a new Comparison to deal with this type.
            
            var (result, context) = ShouldBeLikeExtensions.CreateTestingComparison().Compare(new ComparisonContext(), new Foo(12), 42);

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

        public class Foo(int value)
        {
            public int Value { get; } = value;
        }
    }
}
