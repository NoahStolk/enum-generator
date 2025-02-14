﻿//HintName: DayOfWeek.g.cs
// <auto-generated>
// This code was generated by EnumGenerator.
// </auto-generated>

#nullable enable

using System.Collections.Generic;

namespace System;

public static partial class DayOfWeekExtensions
{
	public static string ToStringFast(this DayOfWeek value)
	{
		return value switch
		{
			DayOfWeek.Sunday => "Sunday",
			DayOfWeek.Monday => "Monday",
			DayOfWeek.Tuesday => "Tuesday",
			DayOfWeek.Wednesday => "Wednesday",
			DayOfWeek.Thursday => "Thursday",
			DayOfWeek.Friday => "Friday",
			DayOfWeek.Saturday => "Saturday",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
		};
	}

	public static ReadOnlySpan<byte> AsUtf8Span(this DayOfWeek value)
	{
		return value switch
		{
			DayOfWeek.Sunday => "Sunday"u8,
			DayOfWeek.Monday => "Monday"u8,
			DayOfWeek.Tuesday => "Tuesday"u8,
			DayOfWeek.Wednesday => "Wednesday"u8,
			DayOfWeek.Thursday => "Thursday"u8,
			DayOfWeek.Friday => "Friday"u8,
			DayOfWeek.Saturday => "Saturday"u8,
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
		};
	}
}

public static partial class DayOfWeekUtils
{
	public static IReadOnlyList<DayOfWeek> Values { get; } = Enum.GetValues<DayOfWeek>();

	public static ReadOnlySpan<byte> NullTerminatedMemberNames => "Sunday\0Monday\0Tuesday\0Wednesday\0Thursday\0Friday\0Saturday\0"u8;
}
