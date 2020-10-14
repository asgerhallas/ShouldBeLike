using System;
using DeepEqual;

namespace ShouldBeLike
{
    public class ObjectObjectComparion : IComparison
    {
        public bool CanCompare(Type type1, Type type2) => type1.IsClass && type1 == type2;

        public (ComparisonResult result, IComparisonContext context) Compare(IComparisonContext context, object value1, object value2)
        {
            var properties = ReflectionCache.GetProperties(value1);

            if (properties.Length == 0) return (ComparisonResult.Pass, context);

            return (ComparisonResult.Inconclusive, context);
        }
    }
}