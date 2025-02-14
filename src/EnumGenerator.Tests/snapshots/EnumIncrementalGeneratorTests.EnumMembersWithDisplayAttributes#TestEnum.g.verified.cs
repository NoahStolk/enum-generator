﻿//HintName: TestEnum.g.cs
// <auto-generated>
// This code was generated by EnumGenerator.
// </auto-generated>

#nullable enable

using System;
using System.Collections.Generic;

namespace Tests;

internal static class TestEnumUtils
{
	public static IReadOnlyList<TestEnum> Values { get; } = Enum.GetValues<TestEnum>();

	public static ReadOnlySpan<byte> NullTerminatedMemberNames => "C#\0C++\0"u8;

	public static string ToStringFast(this TestEnum value)
	{
		return value switch
		{
			TestEnum.CSharp => "C#",
			TestEnum.CPlusPlus => "C++",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
		};
	}

	public static ReadOnlySpan<byte> AsUtf8Span(this TestEnum value)
	{
		return value switch
		{
			TestEnum.CSharp => "C#"u8,
			TestEnum.CPlusPlus => "C++"u8,
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
		};
	}
}
