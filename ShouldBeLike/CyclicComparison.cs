using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DeepEqual;

namespace ShouldBeLike
{
    public class CyclicComparison : IComparison
    {
        readonly CompositeComparison root;
        readonly HashSet<(object, object)> left = new HashSet<(object, object)>();

        public CyclicComparison(CompositeComparison root) => this.root = root;

        public bool CanCompare(Type type1, Type type2) => true;

        public (ComparisonResult result, IComparisonContext context) Compare(IComparisonContext context, object value1, object value2)
        {
            if (!left.Add((value1, value2))) return (ComparisonResult.Pass, context);

            var rest = new CompositeComparison(root.Comparisons.Except(new[] {this}));

            return rest.Compare(context, value1, value2);
        }

        class Comparer : IEqualityComparer
        {
            readonly IComparison comparison;
            readonly IComparisonContext context;
            readonly List<ComparisonResult> results = new List<ComparisonResult>();

            public Comparer(IComparison comparison, IComparisonContext context)
            {
                this.comparison = comparison;
                this.context = context;
            }

            public IReadOnlyList<ComparisonResult> Results => results;

            public bool Equals(object x, object y)
            {
                var result = comparison.Compare(context, x, y);
                results.Add(result.result);

                // Objects without properties is compared to inconclusive by default by DeepEqual library, for testing Inconclusive means pass.
                // Equals cannot return Inconclusive, so this comparer would always either pass or fail and not continue to the next comparer for
                // further inspection. Therefore we always return true and consult the Results property afterwards.
                // Later we can adopt the DeepEquals library and change the default behavior for empty objects, so that they will pass instead of
                // be inconclusive when they are compared.
                return true;
            }

            public int GetHashCode(object obj) => obj.GetHashCode();
        }
    }
}