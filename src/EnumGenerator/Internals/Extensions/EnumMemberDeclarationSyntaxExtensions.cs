using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EnumGenerator.Internals.Extensions;

internal static class EnumMemberDeclarationSyntaxExtensions
{
	public static string? GetConstantValue(this EnumMemberDeclarationSyntax member)
	{
		return member.EqualsValue?.Value is LiteralExpressionSyntax numericLiteral ? numericLiteral.Token.ValueText : null;
	}
}
