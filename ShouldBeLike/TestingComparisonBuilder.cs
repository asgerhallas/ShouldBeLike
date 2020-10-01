using DeepEqual;

namespace ShouldBeLike
{
    public class TestingComparisonBuilder : ComparisonBuilder
    {
        public TestingComparisonBuilder()
        {
            WithCustomComparison(new CyclesComparison(Root));
            WithCustomComparison(new StructuralEquatableComparison(Root));
        }
    }
}