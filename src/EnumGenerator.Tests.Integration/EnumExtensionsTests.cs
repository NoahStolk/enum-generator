using EnumGenerator.Sample.Enums;
using EnumGenerator.Tests.Integration.Enums;
using Xunit;

namespace EnumGenerator.Tests.Integration;

public sealed class EnumExtensionsTests
{
	[Fact]
	public void ToStringFastReturnsCorrectResult()
	{
		Assert.Equal("Byte", IntegerType.Byte.ToStringFast());
		Assert.Equal("Short", IntegerType.Short.ToStringFast());
		Assert.Equal("Int", IntegerType.Int.ToStringFast());
		Assert.Equal("Long", IntegerType.Long.ToStringFast());

		Assert.Equal("C#", Language.CSharp.ToStringFast());
		Assert.Equal("C++", Language.CPlusPlus.ToStringFast());
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
}
