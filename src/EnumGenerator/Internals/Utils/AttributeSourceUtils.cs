namespace EnumGenerator.Internals.Utils;

/// <summary>
/// The attributes are emitted into the consuming compilation instead of being shipped as a reference assembly.
/// This keeps the NuGet package a true development dependency (no <c>lib/</c> folder, so no <c>compile</c> asset is needed)
/// and leaves consumers without any runtime dependency on EnumGenerator.
/// </summary>
internal static class AttributeSourceUtils
{
	public const string HintName = $"{GeneratorConstants.GenerateEnumUtilitiesAttributeName}.g.cs";

	/// <remarks>
	/// The attributes are <c>internal</c> so that assemblies which both use the generator do not end up exposing
	/// conflicting public types to each other. Everything is fully qualified because the consuming compilation
	/// may not have <c>ImplicitUsings</c> enabled.
	/// </remarks>
	public const string SourceCode =
		$$"""
		  namespace {{GeneratorConstants.RootNamespace}};

		  [global::System.AttributeUsage(global::System.AttributeTargets.Enum)]
		  internal sealed class {{GeneratorConstants.GenerateEnumUtilitiesAttributeName}} : global::System.Attribute
		  {
		  	public {{GeneratorConstants.GenerateEnumUtilitiesAttributeName}}()
		  	{
		  	}

		  	public {{GeneratorConstants.GenerateEnumUtilitiesAttributeName}}(string? generatedClassName)
		  	{
		  		GeneratedClassName = generatedClassName;
		  	}

		  	public string? GeneratedClassName { get; set; }
		  }

		  [global::System.AttributeUsage(global::System.AttributeTargets.Assembly, AllowMultiple = true)]
		  internal sealed class {{GeneratorConstants.GenerateEnumUtilitiesAttributeName}}<T> : global::System.Attribute
		  	where T : global::System.Enum
		  {
		  	public {{GeneratorConstants.GenerateEnumUtilitiesAttributeName}}()
		  	{
		  	}

		  	public {{GeneratorConstants.GenerateEnumUtilitiesAttributeName}}(string? generatedClassName)
		  	{
		  		GeneratedClassName = generatedClassName;
		  	}

		  	public string? GeneratedClassName { get; set; }
		  }
		  """;
}
