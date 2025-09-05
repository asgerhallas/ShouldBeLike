using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DeepEqual;

namespace ShouldBeLike;

public class FailOnDifferentTypes : IComparison
{
    static readonly IReadOnlyList<Type> listTypes = new List<Type>
    {
        typeof(List<>),
        typeof(Collection<>)
    };

    public bool CanCompare(Type type1, Type type2) => true;

    public (ComparisonResult result, IComparisonContext context) Compare(IComparisonContext context, object value1, object value2)
    {
        var type1 = value1.GetType();
        var type2 = value2.GetType();

        var xType1 = type1.IsGenericType ? type1.GetGenericTypeDefinition() : type1;
        var xType2 = type2.IsGenericType ? type2.GetGenericTypeDefinition() : type2;

        var canBeConsideredSameType = (listTypes.Contains(xType1) || xType1.IsArray) && 
                                      (listTypes.Contains(xType2) || xType2.IsArray);

        if (type1 != type2 && !canBeConsideredSameType) return (ComparisonResult.Fail, context);

        return (ComparisonResult.Inconclusive, context);
    }
}