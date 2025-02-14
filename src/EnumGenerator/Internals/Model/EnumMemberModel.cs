namespace EnumGenerator.Internals.Model;

internal sealed record EnumMemberModel
{
	public required string? ExplicitValue { get; init; }

	public required string Name { get; init; }

	public required string DisplayName { get; init; }
}
