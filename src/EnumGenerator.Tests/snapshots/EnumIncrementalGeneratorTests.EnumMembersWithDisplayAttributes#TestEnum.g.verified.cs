﻿//HintName: TestEnum.g.cs
// <auto-generated>
// This code was generated by EnumGenerator.
// </auto-generated>

#nullable enable

using System;
using System.Collections.Generic;
using System.IO;

namespace Tests;

internal static class TestEnumGen
{
	public static IReadOnlyList<Tests.TestEnum> Values { get; } = Enum.GetValues<Tests.TestEnum>();

	public static ReadOnlySpan<byte> NullTerminatedMemberNames => "C#\0C++\0"u8;

	public static string ToStringFast(this Tests.TestEnum value)
	{
		return value switch
		{
			Tests.TestEnum.CSharp => "C#",
			Tests.TestEnum.CPlusPlus => "C++",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
		};
	}

	public static ReadOnlySpan<byte> AsUtf8Span(this Tests.TestEnum value)
	{
		return value switch
		{
			Tests.TestEnum.CSharp => "C#"u8,
			Tests.TestEnum.CPlusPlus => "C++"u8,
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
		};
	}

	public static int GetIndex(this Tests.TestEnum value)
	{
		return value switch
		{
			Tests.TestEnum.CSharp => 0,
			Tests.TestEnum.CPlusPlus => 1,
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
		};
	}

	public static Tests.TestEnum FromIndex(int index)
	{
		return index switch
		{
			0 => Tests.TestEnum.CSharp,
			1 => Tests.TestEnum.CPlusPlus,
			_ => throw new ArgumentOutOfRangeException(nameof(index), index, null),
		};
	}

	public static void Write(this BinaryWriter writer, Tests.TestEnum value)
	{
		writer.Write((int)value);
	}

	public static Tests.TestEnum ReadTestEnum(this BinaryReader reader)
	{
		return (Tests.TestEnum)reader.ReadInt32();
	}
}
