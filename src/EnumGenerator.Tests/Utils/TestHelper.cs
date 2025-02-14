using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Immutable;
using System.Reflection;

namespace EnumGenerator.Tests.Utils;

internal static class TestHelper
{
	private static readonly CSharpCompilationOptions _compilationOptions = new(
		outputKind: OutputKind.DynamicallyLinkedLibrary,
		generalDiagnosticOption: ReportDiagnostic.Warn,
		nullableContextOptions: NullableContextOptions.Enable);

	public static Task Verify(string source)
	{
		Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
		Assembly netstandard = assemblies.Single(a => a.GetName().Name == "netstandard");
		Assembly systemRuntime = assemblies.Single(a => a.GetName().Name == "System.Runtime");

		SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(source);
		CSharpCompilation compilation = CSharpCompilation.Create(
			assemblyName: "EnumGenerator.Tests",
			syntaxTrees: [syntaxTree],
			references:
			[
				MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
				MetadataReference.CreateFromFile(typeof(GenerateEnumUtilitiesAttribute).Assembly.Location),
				MetadataReference.CreateFromFile(netstandard.Location),
				MetadataReference.CreateFromFile(systemRuntime.Location),
			],
			options: _compilationOptions);

		EnumIncrementalGenerator generator = new();
		GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);
		driver = driver.RunGeneratorsAndUpdateCompilation(compilation, out Compilation outputCompilation, out _);

		ImmutableArray<Diagnostic> diagnostics = outputCompilation.GetDiagnostics();
		if (diagnostics.Length > 0)
			return Task.FromException(new InvalidOperationException($"Post-generator compilation failed ({diagnostics.Length} errors):\n{string.Join(Environment.NewLine, diagnostics)}"));

		return Verifier.Verify(driver).UseDirectory(Path.Combine("..", "snapshots"));
	}
}
