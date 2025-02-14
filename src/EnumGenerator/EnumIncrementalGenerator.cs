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
		IncrementalValuesProvider<EnumModel> unionModelProvider = context.SyntaxProvider
			.CreateSyntaxProvider(
				(sn, _) => sn is EnumDeclarationSyntax,
				(ctx, _) => GetEnumModel(ctx))
			.Where(um => um != null)
			.Select((um, _) => um!);

		context.RegisterSourceOutput(unionModelProvider.Collect(), GenerateEnumUtilities);
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
