using EnumGenerator.Tests.Utils;

namespace EnumGenerator.Tests;

public sealed class EnumIncrementalGeneratorTests
{
	[Fact]
	public async Task Enum()
	{
		const string code =
			"""
			using EnumGenerator;
			namespace Tests;
			[GenerateEnumUtilities]
			internal enum TestEnum
			{
				None,
				First,
				Second,
				Third,
				Fourth,
				Fifth,
			}
			""";

		await TestHelper.Verify(code);
	}

	[Fact]
	public async Task PublicEnum()
	{
		const string code =
			"""
			using EnumGenerator;
			namespace Tests;
			[GenerateEnumUtilities]
			public enum TestEnum
			{
				None,
				First,
				Second,
				Third,
				Fourth,
				Fifth,
			}
			""";

		await TestHelper.Verify(code);
	}

	[Fact]
	public async Task EnumWithByteUnderlyingType()
	{
		const string code =
			"""
			using EnumGenerator;
			namespace Tests;
			[GenerateEnumUtilities]
			internal enum TestEnum : byte
			{
				None,
				First,
				Second,
				Third,
				Fourth,
				Fifth,
			}
			""";

		await TestHelper.Verify(code);
	}

	[Fact]
	public async Task EnumWithFlagsAttribute()
	{
		const string code =
			"""
			using System;
			using EnumGenerator;
			namespace Tests;
			[Flags]
			[GenerateEnumUtilities]
			internal enum TestEnum
			{
				None = 0,
				First = 1,
				Second = 2,
				Third = 4,
				Fourth = 8,
				Fifth = 16,
			}
			""";

		await TestHelper.Verify(code);
	}

	[Fact]
	public async Task EnumMembersWithDisplayAttributes()
	{
		const string code =
			"""
			using System.ComponentModel.DataAnnotations;
			using EnumGenerator;
			namespace Tests;
			[GenerateEnumUtilities]
			internal enum TestEnum
			{
				[Display(Name = "C#")]
				CSharp,
				[Display(Name = "C++")]
				CPlusPlus,
			}
			""";

		await TestHelper.Verify(code);
	}

	[Fact]
	public async Task ExternalEnum()
	{
		const string code =
			"""
			using System;
			using EnumGenerator;
			[assembly: GenerateEnumUtilities<DayOfWeek>]
			""";

		await TestHelper.Verify(code);
	}

	[Fact]
	public async Task ExternalEnums()
	{
		const string code =
			"""
			using System;
			using EnumGenerator;
			[assembly: GenerateEnumUtilities<DayOfWeek>]
			[assembly: GenerateEnumUtilities<DateTimeKind>]
			""";

		await TestHelper.Verify(code);
	}
}
