using EnumGenerator.Internals.Extensions;
using EnumGenerator.Internals.Model;
using EnumGenerator.Internals.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EnumGenerator.Internals.ModelBuilders;

internal sealed class EnumModelBuilder(SemanticModel semanticModel, EnumDeclarationSyntax enumDeclarationSyntax, INamedTypeSymbol enumSymbol)
{
	public EnumModel Build()
	{
		return new EnumModel
		{
			EnumName = enumDeclarationSyntax.Identifier.Text,
			EnumTypeName = enumSymbol.ToDisplayString(),
			NamespaceName = enumSymbol.ContainingNamespace.ToDisplayString(),
			Accessibility = enumSymbol.DeclaredAccessibility.ToString().ToLowerInvariant(),
			Members = GetMemberNames(),
			HasFlagsAttribute = enumDeclarationSyntax.HasAttribute(semanticModel, "System.FlagsAttribute"),
			GeneratedClassName = enumDeclarationSyntax.GetAttributeArgumentValue<string>(semanticModel, $"{GeneratorConstants.RootNamespace}.{GeneratorConstants.GenerateEnumUtilitiesAttributeName}", "GeneratedClassName"),
			EnumUnderlyingTypeName = GetEnumUnderlyingTypeString(enumSymbol.EnumUnderlyingType),
			BinaryReaderMethodName = GetBinaryReaderMethodName(enumSymbol.EnumUnderlyingType),
		};
	}

	private List<EnumMemberModel> GetMemberNames()
	{
		List<EnumMemberModel> memberNames = [];
		foreach (EnumMemberDeclarationSyntax member in enumDeclarationSyntax.Members)
		{
			string displayName = member.GetAttributeArgumentValue<string>(semanticModel, "System.ComponentModel.DataAnnotations.DisplayAttribute", "Name") ?? member.Identifier.Text;

			memberNames.Add(new EnumMemberModel
			{
				Name = member.Identifier.Text,
				DisplayName = displayName,
				ConstantValue = member.GetConstantValue(),
			});
		}

		return memberNames;
	}

	public static EnumModel BuildFromCompilation(ITypeSymbol enumTypeSymbol, string? generatedClassName, INamedTypeSymbol namedTypeSymbol)
	{
		return new EnumModel
		{
			EnumName = enumTypeSymbol.Name,
			EnumTypeName = enumTypeSymbol.ToDisplayString(),
			NamespaceName = enumTypeSymbol.ContainingNamespace.ToDisplayString(),
			Accessibility = "public",
			Members = enumTypeSymbol
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
			HasFlagsAttribute = enumTypeSymbol.GetAttributes().Any(a => a.AttributeClass?.Name == "FlagsAttribute"),
			GeneratedClassName = generatedClassName,
			EnumUnderlyingTypeName = GetEnumUnderlyingTypeString(namedTypeSymbol.EnumUnderlyingType),
			BinaryReaderMethodName = GetBinaryReaderMethodName(namedTypeSymbol.EnumUnderlyingType),
		};
	}

	private static string GetEnumUnderlyingTypeString(INamedTypeSymbol? enumUnderlyingType)
	{
		return enumUnderlyingType?.ToDisplayString() ?? "int";
	}

	private static string GetBinaryReaderMethodName(INamedTypeSymbol? enumUnderlyingType)
	{
		string typeString = GetEnumUnderlyingTypeString(enumUnderlyingType);
		return typeString switch
		{
			"byte" => "ReadByte",
			"ushort" => "ReadUInt16",
			"uint" => "ReadUInt32",
			"ulong" => "ReadUInt64",
			"sbyte" => "ReadSByte",
			"short" => "ReadInt16",
			"int" => "ReadInt32",
			"long" => "ReadInt64",
			_ => throw new ArgumentOutOfRangeException(nameof(enumUnderlyingType), typeString, null),
		};
	}
}
