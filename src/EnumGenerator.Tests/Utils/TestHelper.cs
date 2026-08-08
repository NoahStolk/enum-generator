using EnumGenerator.Internals.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace EnumGenerator.Tests.Utils;

internal static class TestHelper
{
	private static readonly CSharpCompilationOptions _compilationOptions = new(
		outputKind: OutputKind.DynamicallyLinkedLibrary,
		generalDiagnosticOption: ReportDiagnostic.Warn,
		nullableContextOptions: NullableContextOptions.Enable);

	/// <summary>
	/// Verifies the utilities generated for <paramref name="source"/>. The attributes the generator adds during
	/// post-initialization are excluded, since they are identical for every compilation;
	/// <see cref="VerifyIncludingAttributes"/> covers those.
	/// </summary>
	public static Task Verify(string source, params string[] args)
	{
		return Verify(source, includeAttributes: false, args);
	}

	/// <summary>Verifies the attributes the generator emits into the consuming compilation.</summary>
	public static Task VerifyIncludingAttributes(string source)
	{
		return Verify(source, includeAttributes: true, []);
	}

	private static Task Verify(string source, bool includeAttributes, string[] args)
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
				MetadataReference.CreateFromFile(netstandard.Location),
				MetadataReference.CreateFromFile(systemRuntime.Location),
				MetadataReference.CreateFromFile(typeof(DisplayAttribute).Assembly.Location),
			],
			options: _compilationOptions);

		EnumIncrementalGenerator generator = new();
		GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);
		driver = driver.RunGeneratorsAndUpdateCompilation(compilation, out Compilation outputCompilation, out _);

		ImmutableArray<Diagnostic> diagnostics = outputCompilation.GetDiagnostics();
		if (diagnostics.Length > 0)
			return Task.FromException(new InvalidOperationException($"Post-generator compilation failed ({diagnostics.Length} errors):\n{string.Join(Environment.NewLine, diagnostics)}"));

		SettingsTask settingsTask = Verifier.Verify(driver).UseDirectory(Path.Combine("..", "snapshots"));
		if (!includeAttributes)
			settingsTask = settingsTask.IgnoreGeneratedResult(r => r.HintName == AttributeSourceUtils.HintName);

		if (args.Length > 0)
			settingsTask = settingsTask.UseParameters(args);

		return settingsTask;
	}
}
