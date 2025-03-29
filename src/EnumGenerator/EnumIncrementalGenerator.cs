using EnumGenerator.Internals;
using EnumGenerator.Internals.Extensions;
using EnumGenerator.Internals.Model;
using EnumGenerator.Internals.ModelBuilders;
using EnumGenerator.Internals.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.Text;

namespace EnumGenerator;

[Generator]
public sealed class EnumIncrementalGenerator : IIncrementalGenerator
{
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		// ! LINQ is used to filter out null values.
		IncrementalValuesProvider<EnumModel> enumModelProvider = context.SyntaxProvider
			.CreateSyntaxProvider(
				(sn, _) => sn is EnumDeclarationSyntax,
				(ctx, _) => GetEnumModel(ctx))
			.Where(em => em != null)
			.Select((em, _) => em!);

		IncrementalValuesProvider<EnumModel> attributeEnumModelProvider = context.CompilationProvider
			.SelectMany((compilation, _) => GetEnumModelsFromAttributes(compilation));

		context.RegisterSourceOutput(attributeEnumModelProvider.Collect(), GenerateEnumUtilities);
		context.RegisterSourceOutput(enumModelProvider.Collect(), GenerateEnumUtilities);
	}

	private static EnumModel? GetEnumModel(GeneratorSyntaxContext context)
	{
		EnumDeclarationSyntax enumDeclarationSyntax = (EnumDeclarationSyntax)context.Node;
		if (context.SemanticModel.GetDeclaredSymbol(enumDeclarationSyntax) is not INamedTypeSymbol enumSymbol)
			return null;

		if (!enumDeclarationSyntax.HasAttribute(context.SemanticModel, $"{GeneratorConstants.RootNamespace}.{GeneratorConstants.GenerateEnumUtilitiesAttributeName}"))
			return null;

		EnumModelBuilder builder = new(context.SemanticModel, enumDeclarationSyntax, enumSymbol);
		return builder.Build();
	}

	private static List<EnumModel> GetEnumModelsFromAttributes(Compilation compilation)
	{
		List<EnumModel> enumModels = [];

		foreach (AttributeData? attribute in compilation.Assembly.GetAttributes())
		{
			if (attribute.AttributeClass is not { Name: "GenerateEnumUtilitiesAttribute", IsGenericType: true })
				continue;

			ITypeSymbol enumType = attribute.AttributeClass.TypeArguments[0];
			if (enumType.TypeKind != TypeKind.Enum)
				continue;

			string? generatedClassName = attribute.NamedArguments.FirstOrDefault(kvp => kvp.Key == "GeneratedClassName").Value.Value?.ToString();

			enumModels.Add(new EnumModel
			{
				EnumName = enumType.Name,
				EnumTypeName = enumType.ToDisplayString(),
				NamespaceName = enumType.ContainingNamespace.ToDisplayString(),
				Accessibility = "public",
				Members = enumType
					.GetMembers()
					.Where(m => m.Kind == SymbolKind.Field)
					.Select(m =>
					{
						IFieldSymbol fieldSymbol = (IFieldSymbol)m;
						return new EnumMemberModel
						{
							ConstantValue = fieldSymbol.ConstantValue?.ToString(),
							Name = m.Name,
							DisplayName = m.Name,
						};
					})
					.ToList(),
				HasFlagsAttribute = enumType.GetAttributes().Any(a => a.AttributeClass?.Name == "FlagsAttribute"),
				GeneratedClassName = generatedClassName,
			});
		}

		return enumModels;
	}

	private static void GenerateEnumUtilities(SourceProductionContext context, ImmutableArray<EnumModel> enumModels)
	{
		foreach (EnumModel enumModel in enumModels)
		{
			EnumCodeGenerator codeGenerator = new(enumModel);
			string sourceCode = SourceBuilderUtils.Build(codeGenerator.Generate());

			context.AddSource($"{enumModel.EnumName}.g.cs", SourceText.From(sourceCode, Encoding.UTF8));
		}
	}
}
