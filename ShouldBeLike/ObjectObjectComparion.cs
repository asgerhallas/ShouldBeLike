using System;
using DeepEqual;

namespace ShouldBeLike
{
    public class ObjectObjectComparion : IComparison
    {
        public bool CanCompare(Type type1, Type type2) => type1 == typeof(object) && type1 == type2;

        public (ComparisonResult result, IComparisonContext context) Compare(IComparisonContext context, object value1, object value2) => (ComparisonResult.Pass, context);
    }
}