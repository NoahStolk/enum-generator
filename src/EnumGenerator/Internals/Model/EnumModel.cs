﻿namespace EnumGenerator.Internals.Model;

internal sealed record EnumModel
{
	public required string EnumName { get; init; }

	public required string EnumTypeName { get; init; }

	public required string NamespaceName { get; init; }

	public required string Accessibility { get; init; }

	public required List<EnumMemberModel> Members { get; init; }

	public required bool HasFlagsAttribute { get; init; }

	public required string? GeneratedClassName { get; init; }

	public required string EnumUnderlyingTypeName { get; init; }

	public required string BinaryReaderMethodName { get; init; }
}
