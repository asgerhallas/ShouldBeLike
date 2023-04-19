namespace ShouldBeLike
{
    using System;
    using DeepEqual;
    using Newtonsoft.Json.Linq;
    using ComparisonResult = DeepEqual.ComparisonResult;
    using IComparison = DeepEqual.IComparison;
    using IComparisonContext = DeepEqual.IComparisonContext;

    public class JTokenComparison : IComparison
    {
        public bool CanCompare(Type type1, Type type2) => typeof(JToken).IsAssignableFrom(type1) && type1 == type2;

        public (ComparisonResult result, IComparisonContext context) Compare(IComparisonContext context, object value1, object value2)
        {
            var t1 = (JToken)value1;
            var t2 = (JToken)value2;

            if (JToken.DeepEquals(t1, t2)) return (ComparisonResult.Pass, context);

            return (ComparisonResult.Fail, context.AddDifference(new BasicDifference(context.Breadcrumb, t1, t2, "dunno yet")));
        }
    }
}