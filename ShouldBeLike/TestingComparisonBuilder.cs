using DeepEqual;

namespace ShouldBeLike
{
    public class TestingComparisonBuilder : ComparisonBuilder
    {
        public TestingComparisonBuilder()
        {
            WithCustomComparison(new StructuralEquatableComparison(Root));
        }
    }
}