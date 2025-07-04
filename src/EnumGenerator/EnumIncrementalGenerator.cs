﻿using EnumGenerator.Internals;
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

			ITypeSymbol enumTypeSymbol = attribute.AttributeClass.TypeArguments[0];
			if (enumTypeSymbol.TypeKind != TypeKind.Enum)
				continue;

			if (enumTypeSymbol is not INamedTypeSymbol namedTypeSymbol)
				continue;

			string? generatedClassName = attribute.NamedArguments.FirstOrDefault(kvp => kvp.Key == "GeneratedClassName").Value.Value?.ToString();

			enumModels.Add(EnumModelBuilder.BuildFromCompilation(generatedClassName, namedTypeSymbol));
		}

		return enumModels;
	}

	private static void GenerateEnumUtilities(SourceProductionContext context, ImmutableArray<EnumModel> enumModels)
	{
		foreach (EnumModel enumModel in enumModels)
		{
			string sourceCode;
			if (enumModel.HasFlagsAttribute)
			{
				FlagsEnumCodeGenerator codeGenerator = new(enumModel);
				sourceCode = SourceBuilderUtils.Build(codeGenerator.Generate());
			}
			else
			{
				EnumCodeGenerator codeGenerator = new(enumModel);
				sourceCode = SourceBuilderUtils.Build(codeGenerator.Generate());
			}

			context.AddSource($"{enumModel.EnumName}.g.cs", SourceText.From(sourceCode, Encoding.UTF8));
		}
	}
}
