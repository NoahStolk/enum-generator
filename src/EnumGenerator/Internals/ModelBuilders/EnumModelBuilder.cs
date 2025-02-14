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
			HasFlagsAttribute = enumDeclarationSyntax.HasAttribute(semanticModel, "System.FlagsAttribute"),
		};
	}

	private Dictionary<string, string> GetMemberNames()
	{
		Dictionary<string, string> memberNames = [];
		foreach (EnumMemberDeclarationSyntax member in enumDeclarationSyntax.Members)
		{
			string memberName = member.GetAttributeArgumentValue<string>(semanticModel, "System.ComponentModel.DataAnnotations.DisplayAttribute", "Name") ?? member.Identifier.Text;
			memberNames.Add(member.Identifier.Text, memberName);
		}

		return memberNames;
	}
}
