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

	[Fact]
	public async Task EnumWithDuplicateMembers()
	{
		const string code =
			"""
			using EnumGenerator;
			namespace Tests;
			[GenerateEnumUtilities]
			public enum BlendEquationModeEXT
			{
				FuncAdd = 0x8006,
				FuncAddExt = 0x8006,
				Min = 0x8007,
				MinExt = 0x8007,
				Max = 0x8008,
				MaxExt = 0x8008,
				FuncSubtract = 0x800A,
				FuncSubtractExt = 0x800A,
				FuncReverseSubtract = 0x800B,
				FuncReverseSubtractExt = 0x800B,
				AlphaMinSgix = 0x8320,
				AlphaMaxSgix = 0x8321,
			}
			""";

		await TestHelper.Verify(code);
	}

	[Fact]
	public async Task ExternalEnumWithDuplicateMembers()
	{
		const string code =
			"""
			using EnumGenerator;
			using Tests;
			[assembly: GenerateEnumUtilities<BlendEquationModeEXT>]
			namespace Tests;
			public enum BlendEquationModeEXT
			{
				FuncAdd = 0x8006,
				FuncAddExt = 0x8006,
				Min = 0x8007,
				MinExt = 0x8007,
				Max = 0x8008,
				MaxExt = 0x8008,
				FuncSubtract = 0x800A,
				FuncSubtractExt = 0x800A,
				FuncReverseSubtract = 0x800B,
				FuncReverseSubtractExt = 0x800B,
				AlphaMinSgix = 0x8320,
				AlphaMaxSgix = 0x8321,
			}
			""";

		await TestHelper.Verify(code);
	}

	[Theory]
	[InlineData("byte")]
	[InlineData("sbyte")]
	[InlineData("short")]
	[InlineData("ushort")]
	[InlineData("int")]
	[InlineData("uint")]
	[InlineData("long")]
	[InlineData("ulong")]
	public async Task EnumWithDuplicateMembersFormattedDifferently(string underlyingType)
	{
		string code =
			$$"""
			  using EnumGenerator;
			  namespace Tests;
			  [GenerateEnumUtilities]
			  public enum TestEnum : {{underlyingType}}
			  {
			  	Member0 = 0,
			  	Member0Ext = 0x00,
			  	Member1 = 1,
			  	Member1Ext = 0x01,
			  }
			  """;

		await TestHelper.Verify(code, underlyingType);
	}
}
