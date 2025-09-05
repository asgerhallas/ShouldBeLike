using System;
using System.Collections.Generic;
using System.Linq;
using DeepEqual;

namespace ShouldBeLike
{
    public class CyclesComparison(CompositeComparison root) : IComparison
    {
        readonly HashSet<(object, object)> left = new();

        public bool CanCompare(Type type1, Type type2) => true;

        public (ComparisonResult result, IComparisonContext context) Compare(IComparisonContext context, object value1, object value2)
        {
            if (!left.Add((value1, value2))) return (ComparisonResult.Pass, context);

            var rest = new CompositeComparison(root.Comparisons.SkipUntil(this).Skip(1));

            return rest.Compare(context, value1, value2);
        }
    }
}