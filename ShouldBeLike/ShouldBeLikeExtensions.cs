using System;
using System.Collections.Generic;
using System.Linq;
using DeepEqual;
using DeepEqual.Formatting;
using DeepEqual.Syntax;

namespace ShouldBeLike
{
    public static class ShouldBeLikeExtensions
    {
        static Action<ComparisonBuilder> setup;

        public static Action<ComparisonBuilder> Setup
        {
            get => setup;
            set 
            {
                if (setup != null)
                {
                    throw new InvalidOperationException("Cannot change setup more than once. Set it up statically.");
                }

                setup = value; 
            }
        }

        public static ComparisonBuilder CreateTestingComparisonBuilder()
        {
            var builder = new ComparisonBuilder();

            TestingComparisonBuilder.Setup(builder);

            Setup?.Invoke(builder);

            return builder;
        }

        public static IComparison CreateTestingComparison() => CreateTestingComparisonBuilder().Create();

        public static void ShouldBeLike<T>(this T actual, T expected) => actual.ShouldBeLike(expected, CreateTestingComparison());

        public static void ShouldBeLike<T>(this IEnumerable<T> actual, params T[] expected) => ShouldBeLike(actual, expected.AsEnumerable());

        public static void ShouldBeLike<T>(this T actual, T expected, IComparison comparison)
        {
            var (comparisonResult, context) = comparison.Compare(new ComparisonContext(), actual, expected);

            if (comparisonResult == ComparisonResult.Inconclusive) 
                throw new InvalidOperationException("Result was inconclusive. This should have been dealt with by the FailOnInconclusiveComparison, but strangely wasn't.");

            if (comparisonResult == ComparisonResult.Pass) return;

            throw new DeepEqualException(
                new DeepEqualExceptionMessageBuilder(
                    context, ComparisonBuilder.Get().GetFormatterFactory()
                ).GetMessage(), context);
        }

        public static void ShouldContain<T>(this IEnumerable<T> actual, T expected, IEqualityComparer<T> comparer)
        {
            var actuals = actual.ToList();

            if (!actuals.Contains(expected, comparer))
            {
                throw new ShouldBeLikeException($"\n{expected} was not present in list:\n\n{string.Join("\n", actuals)}.");
            }
        }

        public static void ShouldBeUnordered<T>(this IEnumerable<T> actual, IEnumerable<T> expected, IEqualityComparer<T> comparer)
        {
            var actuals = actual.ToList();
            var expecteds = expected.ToList();

            var misses = new List<T>();

            foreach (var e in expecteds)
            {
                var a = actuals.Where(x => comparer.Equals(x, e)).Take(1).ToList();

                if (!a.Any()) misses.Add(e);
                else actuals.Remove(a[0]);
            }

            if (actuals.Count == 0 && misses.Count == 0) return;

            var msg = "The given sequences were not the same.";

            if (actuals.Count > 0)
            {
                msg += $@"

List contained these unexpected elements: 

{string.Join("\n", actuals)}";

            }

            if (misses.Count > 0)
            {
                msg += $@"

List did not contain these expected elements: 

{string.Join("\n", misses)}";

            }

            throw new ShouldBeLikeException($"{msg}\n\n");
        }

        public static void ShouldContainLike<T>(this IEnumerable<T> actual, T expected) =>
            actual.ShouldContain(expected, new DeepEqualComparer<T>());

        public static void ShouldContainLike<T>(this IEnumerable<T> actual, params T[] expecteds)
        {
            var actuals = actual.ToList();

            foreach (var expected in expecteds)
                ShouldContainLike(actuals, expected);
        }

        public static void ShouldBeLikeUnordered<T>(this IEnumerable<T> actual, params T[] expecteds) =>
            actual.ShouldBeUnordered(expecteds, new DeepEqualComparer<T>());

        public class DeepEqualComparer<T> : IEqualityComparer<T>
        {
            public bool Equals(T x, T y) => x.IsDeepEqual(y, CreateTestingComparison());
            public int GetHashCode(T obj) => obj.GetHashCode();
        }
    }
}
