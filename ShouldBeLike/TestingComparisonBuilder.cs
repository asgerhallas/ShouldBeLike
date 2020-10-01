using DeepEqual;

namespace ShouldBeLike
{
    public class TestingComparisonBuilder : ComparisonBuilder
    {
        public TestingComparisonBuilder()
        {
            WithCustomComparison(new CyclicComparison(Root));
            WithCustomComparison(new StructuralEquatableComparison(Root));
        }
    }
}