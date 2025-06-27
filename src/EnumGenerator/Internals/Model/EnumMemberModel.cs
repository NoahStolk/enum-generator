using System.Numerics;

namespace EnumGenerator.Internals.Model;

internal sealed record EnumMemberModel
{
	public required BigInteger ConstantValue { get; init; }

	public required string Name { get; init; }

	public required string DisplayName { get; init; }
}
