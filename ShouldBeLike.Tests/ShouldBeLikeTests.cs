using DeepEqual.Syntax;
using Newtonsoft.Json.Linq;
using Xunit;

namespace ShouldBeLike.Tests
{
    public class ShouldBeLikeTests
    {
        //[Fact]
        //public void JObjectShouldBeLike()
        //{
        //    new JObject
        //    {
        //        ["name"] = "Andreas"
        //    }.ShouldBeLike(new JObject
        //    {
        //        ["name"] = "Andreas"
        //    });
        //}

        //[Fact]
        //public void DifferentJObjectFails()
        //{
        //    Assert.Throws<DeepEqualException>(() =>
        //        new JObject
        //        {
        //            ["firstname"] = "Andreas"
        //        }.ShouldBeLike(new JObject
        //        {
        //            ["lastname"] = "Andreas"
        //        }));
        //}

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
    }
}
