﻿//HintName: BlendEquationModeEXT.g.cs
// <auto-generated>
// This code was generated by EnumGenerator.
// </auto-generated>

#nullable enable

using System;
using System.Collections.Generic;

namespace Tests;

public static partial class BlendEquationModeEXTExtensions
{
	public static string ToStringFast(this BlendEquationModeEXT value)
	{
		return value switch
		{
			BlendEquationModeEXT.FuncAdd => "FuncAdd",
			BlendEquationModeEXT.Min => "Min",
			BlendEquationModeEXT.Max => "Max",
			BlendEquationModeEXT.FuncSubtract => "FuncSubtract",
			BlendEquationModeEXT.FuncReverseSubtract => "FuncReverseSubtract",
			BlendEquationModeEXT.AlphaMinSgix => "AlphaMinSgix",
			BlendEquationModeEXT.AlphaMaxSgix => "AlphaMaxSgix",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
		};
	}

	public static ReadOnlySpan<byte> AsUtf8Span(this BlendEquationModeEXT value)
	{
		return value switch
		{
			BlendEquationModeEXT.FuncAdd => "FuncAdd"u8,
			BlendEquationModeEXT.Min => "Min"u8,
			BlendEquationModeEXT.Max => "Max"u8,
			BlendEquationModeEXT.FuncSubtract => "FuncSubtract"u8,
			BlendEquationModeEXT.FuncReverseSubtract => "FuncReverseSubtract"u8,
			BlendEquationModeEXT.AlphaMinSgix => "AlphaMinSgix"u8,
			BlendEquationModeEXT.AlphaMaxSgix => "AlphaMaxSgix"u8,
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
		};
	}
}

public static partial class BlendEquationModeEXTUtils
{
	public static IReadOnlyList<BlendEquationModeEXT> Values { get; } = Enum.GetValues<BlendEquationModeEXT>();

	public static ReadOnlySpan<byte> NullTerminatedMemberNames => "FuncAdd\0Min\0Max\0FuncSubtract\0FuncReverseSubtract\0AlphaMinSgix\0AlphaMaxSgix\0"u8;
}
