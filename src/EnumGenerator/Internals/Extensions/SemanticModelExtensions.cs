using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EnumGenerator.Internals.Extensions;

internal static class SemanticModelExtensions
{
	public static bool HasAttribute(this SemanticModel semanticModel, EnumDeclarationSyntax enumDeclarationSyntax, string attributeFullName)
	{
		foreach (AttributeListSyntax attributeListSyntax in enumDeclarationSyntax.AttributeLists)
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
}
