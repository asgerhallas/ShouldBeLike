using System;
using System.Collections;
using System.Collections.Generic;
using DeepEqual;

namespace ShouldBeLike;

public class FailOnDifferentTypes : IComparison
{
    public bool CanCompare(Type type1, Type type2) => true;

    public (ComparisonResult result, IComparisonContext context) Compare(IComparisonContext context, object value1, object value2)
    {
        var type1 = value1.GetType();
        var type2 = value2.GetType();

        var canBeConsideredSameType = typeof(IEnumerable).IsAssignableFrom(type1) &&
                                      typeof(IEnumerable).IsAssignableFrom(type2);

        if (type1 == type2 || canBeConsideredSameType) return (ComparisonResult.Inconclusive, context);

        return (ComparisonResult.Fail, context.AddDifference(new BasicDifference(context.Breadcrumb, type1, type2, null)));

    }
}