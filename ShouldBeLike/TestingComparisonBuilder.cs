using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DeepEqual;

namespace ShouldBeLike
{
    public static class TestingComparisonBuilder
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

        public static void Setup(ComparisonBuilder builder)
        {
            builder.IgnoreCircularReferences();

            var allComparisons = (CompositeComparison)builder.GetType()
                .GetProperty("AllComparisons", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(builder);

            builder.WithCustomComparison(new FailOnInconclusiveComparison(allComparisons));
            builder.WithCustomComparison(new FailOnDifferentTypes());
            //Builder.WithCustomComparison(new CyclesComparison(allComparisons));
            builder.WithCustomComparison(new StructuralEquatableComparison(allComparisons));
            builder.WithCustomComparison(new EmptyComplexObjectComparison(builder.ComplexObjectComparison));

            foreach (var optionalComparisonType in optionalComparisonTypes)
            {
                builder.WithCustomComparison((IComparison)Activator.CreateInstance(optionalComparisonType));
            }
        }
    }
}