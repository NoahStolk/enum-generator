using EnumGenerator.Internals.Extensions;
using EnumGenerator.Internals.Model;
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
			NamespaceName = enumSymbol.ContainingNamespace.ToDisplayString(),
			Accessibility = enumSymbol.DeclaredAccessibility.ToString().ToLowerInvariant(),
			Members = GetMemberNames(),
			HasFlagsAttribute = semanticModel.HasAttribute(enumDeclarationSyntax, "System.FlagsAttribute"),
		};
	}

	private Dictionary<string, string> GetMemberNames()
	{
		Dictionary<string, string> memberNames = [];
		foreach (EnumMemberDeclarationSyntax member in enumDeclarationSyntax.Members)
			memberNames.Add(member.Identifier.Text, member.Identifier.Text); // TODO: Use DisplayAttribute if available.

		return memberNames;
	}
}
