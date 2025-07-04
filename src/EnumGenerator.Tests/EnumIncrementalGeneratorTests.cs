﻿using EnumGenerator.Tests.Utils;

namespace EnumGenerator.Tests;

public sealed class EnumIncrementalGeneratorTests
{
	[Theory]
	[InlineData("internal", "int")]
	[InlineData("internal", "byte")]
	[InlineData("public", "int")]
	public async Task Enum(string accessibility, string underlyingType)
	{
		string code =
			$$"""
			  using EnumGenerator;
			  namespace Tests;
			  [GenerateEnumUtilities]
			  {{accessibility}} enum TestEnum : {{underlyingType}}
			  {
			  	None,
			  	First,
			  	Second,
			  	Third,
			  	Fourth,
			  	Fifth,
			  }
			  """;

		await TestHelper.Verify(code, accessibility, underlyingType);
	}

	[Theory]
	[InlineData("internal", "int")]
	[InlineData("internal", "byte")]
	[InlineData("public", "int")]
	public async Task EnumWithCustomGeneratedClassName(string accessibility, string underlyingType)
	{
		string code =
			$$"""
			  using EnumGenerator;
			  namespace Tests;
			  [GenerateEnumUtilities(GeneratedClassName = "TestEnumUtilities")]
			  {{accessibility}} enum TestEnum : {{underlyingType}}
			  {
			  	None,
			  	First,
			  	Second,
			  	Third,
			  	Fourth,
			  	Fifth,
			  }
			  """;

		await TestHelper.Verify(code, accessibility, underlyingType);
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
	public async Task ExternalEnumWithCustomGeneratedClassName()
	{
		const string code =
			"""
			using System;
			using EnumGenerator;
			[assembly: GenerateEnumUtilities<DayOfWeek>(GeneratedClassName = "DayOfWeekGeneratedUtilities")]
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

	[Fact]
	public async Task EnumWithDuplicateMembersAndAttributes()
	{
		const string code =
			"""
			using System;
			using EnumGenerator;

			namespace Silk.NET.OpenGL;

			[GenerateEnumUtilities]
			[NativeName("Name", "BlendEquationModeEXT")]
			public enum BlendEquationModeEXT : int
			{
			    [NativeName("Name", "GL_FUNC_ADD")]
			    FuncAdd = 0x8006,
			    [NativeName("Name", "GL_FUNC_ADD_EXT")]
			    FuncAddExt = 0x8006,
			    [NativeName("Name", "GL_MIN")]
			    Min = 0x8007,
			    [NativeName("Name", "GL_MIN_EXT")]
			    MinExt = 0x8007,
			    [NativeName("Name", "GL_MAX")]
			    Max = 0x8008,
			    [NativeName("Name", "GL_MAX_EXT")]
			    MaxExt = 0x8008,
			    [NativeName("Name", "GL_FUNC_SUBTRACT")]
			    FuncSubtract = 0x800A,
			    [NativeName("Name", "GL_FUNC_SUBTRACT_EXT")]
			    FuncSubtractExt = 0x800A,
			    [NativeName("Name", "GL_FUNC_REVERSE_SUBTRACT")]
			    FuncReverseSubtract = 0x800B,
			    [NativeName("Name", "GL_FUNC_REVERSE_SUBTRACT_EXT")]
			    FuncReverseSubtractExt = 0x800B,
			    [NativeName("Name", "GL_ALPHA_MIN_SGIX")]
			    AlphaMinSgix = 0x8320,
			    [NativeName("Name", "GL_ALPHA_MAX_SGIX")]
			    AlphaMaxSgix = 0x8321,
			}

			[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
			public class NativeNameAttribute : Attribute
			{
			    public NativeNameAttribute(string category, string name)
			    {
			        Category = category;
			        Name = name;
			    }

			    public string Category { get; }
			    public string Name { get; }
			}
			""";

		await TestHelper.Verify(code);
	}

	[Fact]
	public async Task ExternalEnumWithDuplicateMembersAndAttributes()
	{
		const string code =
			"""
			using System;
			using EnumGenerator;
			using Silk.NET.OpenGL;

			[assembly: GenerateEnumUtilities<BlendEquationModeEXT>]

			namespace Silk.NET.OpenGL;

			[NativeName("Name", "BlendEquationModeEXT")]
			public enum BlendEquationModeEXT : int
			{
			    [NativeName("Name", "GL_FUNC_ADD")]
			    FuncAdd = 0x8006,
			    [NativeName("Name", "GL_FUNC_ADD_EXT")]
			    FuncAddExt = 0x8006,
			    [NativeName("Name", "GL_MIN")]
			    Min = 0x8007,
			    [NativeName("Name", "GL_MIN_EXT")]
			    MinExt = 0x8007,
			    [NativeName("Name", "GL_MAX")]
			    Max = 0x8008,
			    [NativeName("Name", "GL_MAX_EXT")]
			    MaxExt = 0x8008,
			    [NativeName("Name", "GL_FUNC_SUBTRACT")]
			    FuncSubtract = 0x800A,
			    [NativeName("Name", "GL_FUNC_SUBTRACT_EXT")]
			    FuncSubtractExt = 0x800A,
			    [NativeName("Name", "GL_FUNC_REVERSE_SUBTRACT")]
			    FuncReverseSubtract = 0x800B,
			    [NativeName("Name", "GL_FUNC_REVERSE_SUBTRACT_EXT")]
			    FuncReverseSubtractExt = 0x800B,
			    [NativeName("Name", "GL_ALPHA_MIN_SGIX")]
			    AlphaMinSgix = 0x8320,
			    [NativeName("Name", "GL_ALPHA_MAX_SGIX")]
			    AlphaMaxSgix = 0x8321,
			}

			[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
			public class NativeNameAttribute : Attribute
			{
			    public NativeNameAttribute(string category, string name)
			    {
			        Category = category;
			        Name = name;
			    }

			    public string Category { get; }
			    public string Name { get; }
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

			  	Member2 = 2,
			  	Member3 = Member2 + Member1,
			  	Member4 = Member2 + 2,
			  	Member4Ext = Member2 + 0x02,

			  	Member5 = 0b0001_0000,
			  	Member6 = 0b0000_1000 + Member1Ext + 0x1,
			  }
			  """;

		await TestHelper.Verify(code, underlyingType);
	}

	[Fact]
	public async Task EmptyEnum()
	{
		const string code =
			"""
			using EnumGenerator;
			namespace Tests;
			[GenerateEnumUtilities]
			internal enum TestEnum
			{
			}
			""";

		await TestHelper.Verify(code);
	}

	[Fact]
	public async Task NestedEnum()
	{
		const string code =
			"""
			using EnumGenerator;
			namespace Tests;
			internal class TestClass
			{
				[GenerateEnumUtilities]
				internal enum TestEnum
				{
				}
			}
			""";

		await TestHelper.Verify(code);
	}

	[Fact]
	public async Task ComplexFlagsEnum()
	{
		const string code =
			"""
			using System;
			using EnumGenerator;
			namespace Tests;
			[Flags]
			[GenerateEnumUtilities]
			public enum TestEnum : ushort
			{
				None = 0,

				Entry1 = 0b0000_0001,
				Entry2 = 0b0000_0010,
				Entry3 = 0b0000_0100,
				Entry4 = 0b0000_1000,

				// Bit 5 intentionally left empty.
				Option1 = 0b0010_0000,
				Option2 = 0b0100_0000,
				Option3 = 0b1000_0000,

				Option4 = 0b0000_1000_0000_0000,

				Option1Alias1 = 0b0010_0000,
				Option1Alias2 = Option1Alias1,
				Option1Alias3 = 0x20,
				Option1Alias4 = 32,

				Entry1And2 = Entry1 | Entry2,
				Entry3And4 = Entry3 | Entry4,
				Entry1And3 = Entry1 | Entry3,
				Entry2And4 = Entry2 | Entry4,
			}
			""";

		await TestHelper.Verify(code);
	}

	[Fact]
	public async Task FlagsByte()
	{
		const string code =
			"""
			using System;
			using EnumGenerator;
			namespace Tests;
			[Flags]
			[GenerateEnumUtilities]
			public enum Indices : byte
			{
				None = 0b0000_0000,

				N1 = 0b0000_0001,
				N2 = 0b0000_0010,
				N3 = 0b0000_0100,
				N4 = 0b0000_1000,
				N5 = 0b0001_0000,
				N6 = 0b0010_0000,
				N7 = 0b0100_0000,
				N8 = 0b1000_0000,

				All = 0b1111_1111,
			}
			""";

		await TestHelper.Verify(code);
	}
}
