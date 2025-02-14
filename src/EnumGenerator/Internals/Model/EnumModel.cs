namespace EnumGenerator.Internals.Model;

internal sealed record EnumModel
{
	public required string EnumName { get; init; }

	public required string NamespaceName { get; init; }

	public required string Accessibility { get; init; }

	public required Dictionary<string, string> Members { get; init; }

	public required bool HasFlagsAttribute { get; init; }
}
