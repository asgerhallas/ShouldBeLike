using System;
using System.Linq;
using DeepEqual;

namespace ShouldBeLike
{
    public class FailOnInconclusiveComparison : IComparison
    {
        readonly CompositeComparison root;

        public FailOnInconclusiveComparison(CompositeComparison root) => this.root = root;

        public bool CanCompare(Type type1, Type type2) => true;

        public (ComparisonResult result, IComparisonContext context) Compare(IComparisonContext context, object value1, object value2)
        {
            var rest = new CompositeComparison(root.Comparisons.SkipUntil(this).Skip(1));

            var (result, comparisonContext) = rest.Compare(context, value1, value2);

            if (result != ComparisonResult.Inconclusive) return (result, comparisonContext);
            
            if (value1.GetType() != value2.GetType()) return (ComparisonResult.Fail, context);

            throw new InvalidOperationException("Comparison came up inconclusive.");
        }
    }
}