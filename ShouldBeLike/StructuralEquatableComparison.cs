using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DeepEqual;

namespace ShouldBeLike
{
    public class StructuralEquatableComparison(IComparison root) : IComparison
    {
        public bool CanCompare(Type type1, Type type2) =>
            typeof(IStructuralEquatable).IsAssignableFrom(type1) && type1 == type2;

        public (ComparisonResult result, IComparisonContext context) Compare(IComparisonContext context, object value1, object value2)
        {
            var comparer = new Comparer(root, context);

            ((IStructuralEquatable) value1).Equals(value2, comparer);

            return (comparer.Results.Concat([ComparisonResult.Inconclusive]).Max(), context);
        }

        class Comparer(IComparison comparison, IComparisonContext context) : IEqualityComparer
        {
            readonly List<ComparisonResult> results = new();

            public IReadOnlyList<ComparisonResult> Results => results;

            bool IEqualityComparer.Equals(object x, object y)
            {
                results.Add(comparison.Compare(context, x, y).result);

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