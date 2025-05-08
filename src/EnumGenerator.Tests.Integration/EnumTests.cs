using EnumGenerator.Tests.Integration.Enums;
using Xunit;

namespace EnumGenerator.Tests.Integration;

public sealed class EnumTests
{
	[Fact]
	public void Values()
	{
		Assert.Equal(4, IntegerTypeGen.Values.Count);
		Assert.Equal(IntegerType.Byte, IntegerTypeGen.Values[0]);
		Assert.Equal(IntegerType.Short, IntegerTypeGen.Values[1]);
		Assert.Equal(IntegerType.Int, IntegerTypeGen.Values[2]);
		Assert.Equal(IntegerType.Long, IntegerTypeGen.Values[3]);
	}

	[Fact]
	public void NullTerminatedMemberNames()
	{
		Assert.True("Byte\0Short\0Int\0Long\0"u8.SequenceEqual(IntegerTypeGen.NullTerminatedMemberNames));
	}

	[Fact]
	public void ToStringFastReturnsCorrectResult()
	{
		Assert.Equal("Byte", IntegerType.Byte.ToStringFast());
		Assert.Equal("Short", IntegerType.Short.ToStringFast());
		Assert.Equal("Int", IntegerType.Int.ToStringFast());
		Assert.Equal("Long", IntegerType.Long.ToStringFast());

		Assert.Equal("C#", Language.CSharp.ToStringFast());
		Assert.Equal("C++", Language.CPlusPlus.ToStringFast());

		Assert.Equal("Sunday", DayOfWeek.Sunday.ToStringFast());
		Assert.Equal("Unspecified", DateTimeKind.Unspecified.ToStringFast());
	}

	[Fact]
	public void AsUtf8SpanReturnsCorrectResult()
	{
		Assert.True("Byte"u8.SequenceEqual(IntegerType.Byte.AsUtf8Span()));
		Assert.True("Short"u8.SequenceEqual(IntegerType.Short.AsUtf8Span()));
		Assert.True("Int"u8.SequenceEqual(IntegerType.Int.AsUtf8Span()));
		Assert.True("Long"u8.SequenceEqual(IntegerType.Long.AsUtf8Span()));

		Assert.True("C#"u8.SequenceEqual(Language.CSharp.AsUtf8Span()));
		Assert.True("C++"u8.SequenceEqual(Language.CPlusPlus.AsUtf8Span()));

		Assert.True("Sunday"u8.SequenceEqual(DayOfWeek.Sunday.AsUtf8Span()));
		Assert.True("Unspecified"u8.SequenceEqual(DateTimeKind.Unspecified.AsUtf8Span()));
	}

	[Fact]
	public void HasFlagFastReturnsCorrectResult()
	{
		const FlagsType ab = FlagsType.A | FlagsType.B;
		Assert.True(ab.HasFlagFast(FlagsType.A));
		Assert.True(ab.HasFlagFast(FlagsType.B));
		Assert.False(ab.HasFlagFast(FlagsType.C));
		Assert.False(ab.HasFlagFast(FlagsType.D));
		Assert.False(ab.HasFlagFast(FlagsType.E));
	}

	[Fact]
	public void GetIndexReturnsCorrectResult()
	{
		Assert.Equal(0, IntegerType.Byte.GetIndex());
		Assert.Equal(1, IntegerType.Short.GetIndex());
		Assert.Equal(2, IntegerType.Int.GetIndex());
		Assert.Equal(3, IntegerType.Long.GetIndex());
	}

	[Fact]
	public void GetIndexFlagReturnsCorrectResult()
	{
		Assert.Equal(0, FlagsType.None.GetIndex());
		Assert.Equal(1, FlagsType.A.GetIndex());
		Assert.Equal(2, FlagsType.B.GetIndex());
		Assert.Equal(3, FlagsType.C.GetIndex());
		Assert.Equal(4, FlagsType.D.GetIndex());
		Assert.Equal(5, FlagsType.E.GetIndex());
	}

	[Fact]
	public void FromIndexReturnsCorrectResult()
	{
		Assert.Equal(IntegerType.Byte, IntegerTypeGen.FromIndex(0));
		Assert.Equal(IntegerType.Short, IntegerTypeGen.FromIndex(1));
		Assert.Equal(IntegerType.Int, IntegerTypeGen.FromIndex(2));
		Assert.Equal(IntegerType.Long, IntegerTypeGen.FromIndex(3));
	}

	[Fact]
	public void FromIndexFlagReturnsCorrectResult()
	{
		Assert.Equal(FlagsType.None, FlagsTypeGen.FromIndex(0));
		Assert.Equal(FlagsType.A, FlagsTypeGen.FromIndex(1));
		Assert.Equal(FlagsType.B, FlagsTypeGen.FromIndex(2));
		Assert.Equal(FlagsType.C, FlagsTypeGen.FromIndex(3));
		Assert.Equal(FlagsType.D, FlagsTypeGen.FromIndex(4));
		Assert.Equal(FlagsType.E, FlagsTypeGen.FromIndex(5));
	}
}
