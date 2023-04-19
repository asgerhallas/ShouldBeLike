using System.Diagnostics;
using Microsoft.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ShouldBeLike.SourceGenerator
{
    [Generator]
    public class OptionalComparisonsSourceGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            if (!context.Compilation.SourceModule.ReferencedAssemblySymbols.Any(x => x.Name == "Newtonsoft.Json")) return;

            var resourceName = $"{typeof(OptionalComparisonsSourceGenerator).Namespace}.JTokenComparison.cs";

            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName)!;
            using var reader = new StreamReader(stream);

            var source = reader.ReadToEnd();

            context.AddSource("JTokenComparison.g.cs", source);
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            // No initialization required for this one
        }
    }
}