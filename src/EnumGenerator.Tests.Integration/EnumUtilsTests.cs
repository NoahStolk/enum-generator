using EnumGenerator.Tests.Integration.Enums;
using Xunit;

namespace EnumGenerator.Tests.Integration;

public sealed class EnumUtilsTests
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
}
