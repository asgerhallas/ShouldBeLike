using System;
using System.Collections.Immutable;
using System.Linq;
using DeepEqual;

namespace ShouldBeLike
{
    public class CyclesComparison : IComparison
    {
        readonly CompositeComparison root;

        public CyclesComparison(CompositeComparison root) => this.root = root;

        public bool CanCompare(Type type1, Type type2) => true;

        public (ComparisonResult result, IComparisonContext context) Compare(IComparisonContext context, object value1, object value2)
        {
            var stopper = new CycleStopper(value1, value2);

            if (context.Differences.Contains(stopper))
            {
                return (ComparisonResult.Pass, context);
            }

            var rest = new CompositeComparison(root.Comparisons.SkipUntil(this).Skip(1));

            var (result, resultContext) = rest.Compare(context.AddDifference(stopper), value1, value2);

            return (
                result,
                new ComparisonContext(
                    resultContext.Differences
                        .Except(new[] {stopper})
                        .ToImmutableList(),
                    resultContext.Breadcrumb)
            );
        }

        class CycleStopper : Difference
        {
            readonly (object, object) values;

            public CycleStopper(object value1, object value2) : base(null) => values = (value1, value2);

            bool Equals(CycleStopper other) => Equals(values, other.values);

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;

                return Equals((CycleStopper) obj);
            }

            public override int GetHashCode() => values.GetHashCode();
        }
    }
}