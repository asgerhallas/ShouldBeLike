using DeepEqual.Syntax;
using Xunit;

namespace ShouldBeLike.Tests
{
    public class ShouldBeLikeTests
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

        [Fact]
        public void ObjectShouldBeLike()
        {
            new object().ShouldBeLike(new object());
        }
 
        [Fact]
        public void Cycles()
        {
            new SelfCycling().ShouldBeLike(new SelfCycling());
        }

        [Fact]
        public void Cycles_Null()
        {
            Assert.Throws<DeepEqualException>(() =>
                new SelfCycling().ShouldBeLike(new SelfCycling { Self = null }));
        }

        [Fact]
        public void Cycles_OtherParent()
        {
            new SelfCycling().ShouldBeLike(new SelfCycling { Self = new SelfCycling() });
        }

        [Fact]
        public void Cycles_OtherObject()
        {
            Assert.Throws<DeepEqualException>(() =>
                new SelfCycling().ShouldBeLike(new SelfCycling { Self = new object() }));
        }

        [Fact]
        public void Cycles_Tuples()
        {
            (1, "hello", 2m, new SelfCycling()).ShouldBeLike((1, "hello", 2m, new SelfCycling()));
        }

        [Fact]
        public void Cycles_Multiple()
        {
            var parent = new Parent();
            parent.Child = new Child(parent);
            
            var parent2 = new Parent();
            parent2.Child = new Child(parent2);

            parent.ShouldBeLike(parent2);
        }

        [Fact]
        public void Cycles_Multiple_TupleDownTheChain()
        {
            var parent = new Parent();
            parent.Child = new Child(parent) { Tuple = (1, "asger") };
            
            var parent2 = new Parent();
            parent2.Child = new Child(parent2) { Tuple = (1, "asger") };

            parent.ShouldBeLike(parent2);
        }

        [Fact]
        public void Cycles_Multiple_TupleDownTheChain_NotLike()
        {
            var parent = new Parent();
            parent.Child = new Child(parent) { Tuple = (1, "asger") };
            
            var parent2 = new Parent();
            parent2.Child = new Child(parent2) { Tuple = (1, "peter") };

            Assert.Throws<DeepEqualException>(() =>
                parent.ShouldBeLike(parent2));
        }

        [Fact]
        public void Cycles_Multiple_TupleDownTheChain_WithNewCycles()
        {
            var parent = new Parent();
            parent.Child = new Child(parent) { Tuple = (1, new Child(parent)) };
            
            var parent2 = new Parent();
            parent2.Child = new Child(parent2) { Tuple = (1, new Child(parent2)) };

            parent.ShouldBeLike(parent2);
        }
        
        [Fact]
        public void Cycles_Multiple_TupleDownTheChain_WithNewCycles_NotLike()
        {
            var parent = new Parent();
            parent.Child = new Child(parent) { Tuple = (1, new Child(parent) { Tuple = (1, 2) } ) };
            
            var parent2 = new Parent();
            parent2.Child = new Child(parent2) { Tuple = (1, new Child(parent2) { Tuple = (3, 4) }) };

            Assert.Throws<DeepEqualException>(() =>
                parent.ShouldBeLike(parent2));
        }

        
        public class SelfCycling
        {
            public SelfCycling() => Self = this;

            public object Self { get; set; }
        }

        public class Parent
        {
            public object Child { get; set; }
        }

        public class Child
        {
            public Child(object parent) => Parent = this;

            public object Parent { get; set; }
            public (int, object) Tuple { get; set; }
        }
    }
}
