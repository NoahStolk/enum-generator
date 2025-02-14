using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EnumGenerator.Internals.Extensions;

internal static class MemberDeclarationSyntaxExtensions
{
	public static bool HasAttribute(this MemberDeclarationSyntax memberDeclarationSyntax, SemanticModel semanticModel, string attributeFullName)
	{
		foreach (AttributeListSyntax attributeListSyntax in memberDeclarationSyntax.AttributeLists)
		{
			foreach (AttributeSyntax attributeSyntax in attributeListSyntax.Attributes)
			{
				if (semanticModel.GetSymbolInfo(attributeSyntax).Symbol is not IMethodSymbol attributeSymbol)
					continue;

				if (attributeSymbol.ContainingType.ToDisplayString() != attributeFullName)
					continue;

				return true;
			}
		}

		return false;
	}

	public static T? GetAttributeArgumentValue<T>(this MemberDeclarationSyntax memberDeclarationSyntax, SemanticModel semanticModel, string attributeFullName, string argumentName)
	{
		foreach (AttributeListSyntax attributeListSyntax in memberDeclarationSyntax.AttributeLists)
		{
			foreach (AttributeSyntax attributeSyntax in attributeListSyntax.Attributes)
			{
				if (attributeSyntax.ArgumentList == null)
					continue;

				if (semanticModel.GetSymbolInfo(attributeSyntax).Symbol is not IMethodSymbol attributeSymbol)
					continue;

				if (attributeSymbol.ContainingType.ToDisplayString() != attributeFullName)
					continue;

				foreach (AttributeArgumentSyntax argumentSyntax in attributeSyntax.ArgumentList.Arguments)
				{
					if (argumentSyntax.NameEquals?.Name.Identifier.Text != argumentName)
						continue;

					return (T?)semanticModel.GetConstantValue(argumentSyntax.Expression).Value;
				}
			}
		}

		return default;
	}
}
