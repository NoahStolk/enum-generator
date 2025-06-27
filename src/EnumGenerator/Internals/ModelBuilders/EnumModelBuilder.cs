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
				ConstantValue = member.GetEnumConstantValue(semanticModel),
			});
		}

		return memberNames;
	}

	public static EnumModel BuildFromCompilation(string? generatedClassName, INamedTypeSymbol namedTypeSymbol)
	{
		return new EnumModel
		{
			EnumName = namedTypeSymbol.Name,
			EnumTypeName = namedTypeSymbol.ToDisplayString(),
			NamespaceName = namedTypeSymbol.ContainingNamespace.ToDisplayString(),
			Accessibility = "public",
			Members = namedTypeSymbol
				.GetMembers()
				.OfType<IFieldSymbol>()

				// Note; this "Where" doesn't skip implicitly defined members, as they do technically have a constant value.
				// We only do this to skip fields such as the enum's backing field "value__".
				.Where(f => f.HasConstantValue)
				.Select(m =>
				{
					if (m.ConstantValue == null)
						throw new InvalidOperationException($"Fields without a constant value should have been filtered out. Could not get constant value for '{m.Name}'.");

					return new EnumMemberModel
					{
						ConstantValue = EnumMemberDeclarationSyntaxExtensions.AsBigInteger(m.ConstantValue),
						Name = m.Name,
						DisplayName = m.Name,
					};
				})
				.ToList(),
			HasFlagsAttribute = namedTypeSymbol.GetAttributes().Any(a => a.AttributeClass?.Name == "FlagsAttribute"),
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
