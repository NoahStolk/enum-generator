using EnumGenerator.Tests.Integration.Enums;
using Xunit;

namespace EnumGenerator.Tests.Integration;

public sealed class EnumUtilsTests
{
	[Fact]
	public void Values()
	{
		Assert.Equal(4, IntegerTypeUtils.Values.Count);
		Assert.Equal(IntegerType.Byte, IntegerTypeUtils.Values[0]);
		Assert.Equal(IntegerType.Short, IntegerTypeUtils.Values[1]);
		Assert.Equal(IntegerType.Int, IntegerTypeUtils.Values[2]);
		Assert.Equal(IntegerType.Long, IntegerTypeUtils.Values[3]);
	}

	[Fact]
	public void NullTerminatedMemberNames()
	{
		Assert.True("Byte\0Short\0Int\0Long\0"u8.SequenceEqual(IntegerTypeUtils.NullTerminatedMemberNames));
	}
}
