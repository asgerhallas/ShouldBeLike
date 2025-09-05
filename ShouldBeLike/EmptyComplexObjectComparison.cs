using System;
using System.Linq;
using DeepEqual;

namespace ShouldBeLike
{
    public class EmptyComplexObjectComparison(ComplexObjectComparison complexObjectComparison) : IComparison
    {
        public bool CanCompare(Type type1, Type type2) => type1.IsClass && type1 == type2;

        public (ComparisonResult result, IComparisonContext context) Compare(IComparisonContext context, object value1, object value2)
        {
            var properties = ReflectionCache.GetProperties(value1);

            var nonIgnoredProperties = properties
                .Where(property => 
                    complexObjectComparison.IgnoredProperties.Count == 0 || 
                    complexObjectComparison.IgnoredProperties.Any(x => !x(property)))
                .ToList();

            if (nonIgnoredProperties.Count == 0) return (ComparisonResult.Pass, context);

            return (ComparisonResult.Inconclusive, context);
        }
    }
}