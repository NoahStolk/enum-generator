﻿//HintName: TestEnum.g.cs
// <auto-generated>
// This code was generated by EnumGenerator.
// </auto-generated>

#nullable enable

using System;
using System.Collections.Generic;

namespace Tests;

public static class TestEnumUtils
{
	public static IReadOnlyList<TestEnum> Values { get; } = Enum.GetValues<TestEnum>();

	public static ReadOnlySpan<byte> NullTerminatedMemberNames => "Member0\0Member1\0"u8;

	public static string ToStringFast(this TestEnum value)
	{
		return value switch
		{
			TestEnum.Member0 => "Member0",
			TestEnum.Member1 => "Member1",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
		};
	}

	public static ReadOnlySpan<byte> AsUtf8Span(this TestEnum value)
	{
		return value switch
		{
			TestEnum.Member0 => "Member0"u8,
			TestEnum.Member1 => "Member1"u8,
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
		};
	}
}
