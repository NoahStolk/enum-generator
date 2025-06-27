using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Numerics;

namespace EnumGenerator.Internals.Extensions;

internal static class EnumMemberDeclarationSyntaxExtensions
{
	public static BigInteger GetEnumConstantValue(this EnumMemberDeclarationSyntax member, SemanticModel model)
	{
		IFieldSymbol? symbol = model.GetDeclaredSymbol(member) as IFieldSymbol;
		object? value = symbol?.ConstantValue;
		if (value == null)
			throw new InvalidOperationException($"Could not get constant value for '{member.Identifier.Text}'.");

		return AsBigInteger(value);
	}

	public static BigInteger AsBigInteger(object value)
	{
		return value switch
		{
			byte b => b,
			sbyte sb => sb,
			short s => s,
			ushort us => us,
			int i => i,
			uint ui => ui,
			long l => l,
			ulong ul => ul,
			_ => throw new NotSupportedException($"Unsupported enum value type: {value.GetType()}"),
		};
	}
}
