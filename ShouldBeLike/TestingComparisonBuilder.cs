using System;
using System.Collections.Generic;
using System.Linq;
using DeepEqual;

namespace ShouldBeLike
{
    public class TestingComparisonBuilder : ComparisonBuilder
    {
        static readonly List<Type> optionalComparisonTypes = new();

        static TestingComparisonBuilder()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var comparison = assembly.DefinedTypes.FirstOrDefault(x => x.FullName == "ShouldBeLike.JTokenComparison");

                if (comparison == null) continue;

                optionalComparisonTypes.Add(comparison);
                
                break;
            }
        }

        public TestingComparisonBuilder()
        {
            WithCustomComparison(new CyclesComparison(Root));
            WithCustomComparison(new FailOnInconclusiveComparison(Root));
            WithCustomComparison(new StructuralEquatableComparison(Root));
            WithCustomComparison(new EmptyComplexObjectComparison(ComplexObjectComparison));

            foreach (var optionalComparisonType in optionalComparisonTypes)
            {
                WithCustomComparison((IComparison)Activator.CreateInstance(optionalComparisonType));
            }
        }
    }
}