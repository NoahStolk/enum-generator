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
		Assert.Throws<ArgumentOutOfRangeException>(() => ((DateTimeKind)3).ToStringFast());

		Assert.Equal("None", FlagsType.None.ToStringFast());
		Assert.Equal("A", FlagsType.A.ToStringFast());
		Assert.Equal("B", FlagsType.B.ToStringFast());
		Assert.Equal("C", FlagsType.C.ToStringFast());
		Assert.Equal("D", FlagsType.D.ToStringFast());
		Assert.Equal("E", FlagsType.E.ToStringFast());

		Assert.Equal("A, B", (FlagsType.None | FlagsType.A | FlagsType.B).ToStringFast());
		Assert.Equal("A, B", (FlagsType.A | FlagsType.B).ToStringFast());
		Assert.Equal("A, B, C", (FlagsType.A | FlagsType.B | FlagsType.C).ToStringFast());
		Assert.Equal("B, D", (FlagsType.B | FlagsType.D).ToStringFast());
		Assert.Equal("A, B, C, D, E", (FlagsType.A | FlagsType.B | FlagsType.C | FlagsType.D | FlagsType.E).ToStringFast());
		Assert.Throws<ArgumentOutOfRangeException>(() => ((FlagsType)32).ToStringFast());
		Assert.Throws<ArgumentOutOfRangeException>(() => ((FlagsType)33).ToStringFast());

		Assert.Equal("None", FlagsTypeWithMissingBits.None.ToStringFast());
		Assert.Equal("A", FlagsTypeWithMissingBits.A.ToStringFast());
		Assert.Equal("B", FlagsTypeWithMissingBits.B.ToStringFast());
		Assert.Throws<ArgumentOutOfRangeException>(() => ((FlagsTypeWithMissingBits)4).ToStringFast());
		Assert.Throws<ArgumentOutOfRangeException>(() => ((FlagsTypeWithMissingBits)5).ToStringFast());
		Assert.Throws<ArgumentOutOfRangeException>(() => ((FlagsTypeWithMissingBits)6).ToStringFast());
		Assert.Throws<ArgumentOutOfRangeException>(() => ((FlagsTypeWithMissingBits)7).ToStringFast());
		Assert.Equal("D", FlagsTypeWithMissingBits.D.ToStringFast());
		Assert.Equal("E", FlagsTypeWithMissingBits.E.ToStringFast());

		Assert.Equal("A, B", (FlagsTypeWithMissingBits.None | FlagsTypeWithMissingBits.A | FlagsTypeWithMissingBits.B).ToStringFast());
		Assert.Equal("A, B", (FlagsTypeWithMissingBits.A | FlagsTypeWithMissingBits.B).ToStringFast());
		Assert.Equal("B, D", (FlagsTypeWithMissingBits.B | FlagsTypeWithMissingBits.D).ToStringFast());
		Assert.Throws<ArgumentOutOfRangeException>(() => ((FlagsTypeWithMissingBits)31).ToStringFast());
		Assert.Throws<ArgumentOutOfRangeException>(() => ((FlagsTypeWithMissingBits)32).ToStringFast());
		Assert.Throws<ArgumentOutOfRangeException>(() => ((FlagsTypeWithMissingBits)33).ToStringFast());

		Assert.Equal("No value", NamedFlagsType.None.ToStringFast());
		Assert.Equal("Value A", NamedFlagsType.A.ToStringFast());
		Assert.Equal("Value B", NamedFlagsType.B.ToStringFast());
		Assert.Equal("Value C", NamedFlagsType.C.ToStringFast());
		Assert.Equal("Value D", NamedFlagsType.D.ToStringFast());
		Assert.Equal("Value E", NamedFlagsType.E.ToStringFast());

		Assert.Equal("Value A, Value B", (NamedFlagsType.None | NamedFlagsType.A | NamedFlagsType.B).ToStringFast());
		Assert.Equal("Value A, Value B", (NamedFlagsType.A | NamedFlagsType.B).ToStringFast());
		Assert.Equal("Value A, Value B, Value C", (NamedFlagsType.A | NamedFlagsType.B | NamedFlagsType.C).ToStringFast());
		Assert.Equal("Value B, Value D", (NamedFlagsType.B | NamedFlagsType.D).ToStringFast());
		Assert.Equal("Value A, Value B, Value C, Value D, Value E", (NamedFlagsType.A | NamedFlagsType.B | NamedFlagsType.C | NamedFlagsType.D | NamedFlagsType.E).ToStringFast());
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
		Assert.Throws<ArgumentOutOfRangeException>(() => ((DateTimeKind)3).AsUtf8Span());

		Assert.Equal("None"u8, FlagsType.None.AsUtf8Span());
		Assert.Equal("A"u8, FlagsType.A.AsUtf8Span());
		Assert.Equal("B"u8, FlagsType.B.AsUtf8Span());
		Assert.Equal("C"u8, FlagsType.C.AsUtf8Span());
		Assert.Equal("D"u8, FlagsType.D.AsUtf8Span());
		Assert.Equal("E"u8, FlagsType.E.AsUtf8Span());

		Assert.Equal("A, B"u8, (FlagsType.None | FlagsType.A | FlagsType.B).AsUtf8Span());
		Assert.Equal("A, B"u8, (FlagsType.A | FlagsType.B).AsUtf8Span());
		Assert.Equal("A, B, C"u8, (FlagsType.A | FlagsType.B | FlagsType.C).AsUtf8Span());
		Assert.Equal("B, D"u8, (FlagsType.B | FlagsType.D).AsUtf8Span());
		Assert.Equal("A, B, C, D, E"u8, (FlagsType.A | FlagsType.B | FlagsType.C | FlagsType.D | FlagsType.E).AsUtf8Span());
		Assert.Throws<ArgumentOutOfRangeException>(() => ((FlagsType)32).AsUtf8Span());
		Assert.Throws<ArgumentOutOfRangeException>(() => ((FlagsType)33).AsUtf8Span());

		Assert.Equal("None"u8, FlagsTypeWithMissingBits.None.AsUtf8Span());
		Assert.Equal("A"u8, FlagsTypeWithMissingBits.A.AsUtf8Span());
		Assert.Equal("B"u8, FlagsTypeWithMissingBits.B.AsUtf8Span());
		Assert.Throws<ArgumentOutOfRangeException>(() => ((FlagsTypeWithMissingBits)4).AsUtf8Span());
		Assert.Throws<ArgumentOutOfRangeException>(() => ((FlagsTypeWithMissingBits)5).AsUtf8Span());
		Assert.Throws<ArgumentOutOfRangeException>(() => ((FlagsTypeWithMissingBits)6).AsUtf8Span());
		Assert.Throws<ArgumentOutOfRangeException>(() => ((FlagsTypeWithMissingBits)7).AsUtf8Span());
		Assert.Equal("D"u8, FlagsTypeWithMissingBits.D.AsUtf8Span());
		Assert.Equal("E"u8, FlagsTypeWithMissingBits.E.AsUtf8Span());

		Assert.Equal("A, B"u8, (FlagsTypeWithMissingBits.None | FlagsTypeWithMissingBits.A | FlagsTypeWithMissingBits.B).AsUtf8Span());
		Assert.Equal("A, B"u8, (FlagsTypeWithMissingBits.A | FlagsTypeWithMissingBits.B).AsUtf8Span());
		Assert.Equal("B, D"u8, (FlagsTypeWithMissingBits.B | FlagsTypeWithMissingBits.D).AsUtf8Span());
		Assert.Throws<ArgumentOutOfRangeException>(() => ((FlagsTypeWithMissingBits)31).AsUtf8Span());
		Assert.Throws<ArgumentOutOfRangeException>(() => ((FlagsTypeWithMissingBits)32).AsUtf8Span());
		Assert.Throws<ArgumentOutOfRangeException>(() => ((FlagsTypeWithMissingBits)33).AsUtf8Span());

		Assert.Equal("No value"u8, NamedFlagsType.None.AsUtf8Span());
		Assert.Equal("Value A"u8, NamedFlagsType.A.AsUtf8Span());
		Assert.Equal("Value B"u8, NamedFlagsType.B.AsUtf8Span());
		Assert.Equal("Value C"u8, NamedFlagsType.C.AsUtf8Span());
		Assert.Equal("Value D"u8, NamedFlagsType.D.AsUtf8Span());
		Assert.Equal("Value E"u8, NamedFlagsType.E.AsUtf8Span());

		Assert.Equal("Value A, Value B"u8, (NamedFlagsType.None | NamedFlagsType.A | NamedFlagsType.B).AsUtf8Span());
		Assert.Equal("Value A, Value B"u8, (NamedFlagsType.A | NamedFlagsType.B).AsUtf8Span());
		Assert.Equal("Value A, Value B, Value C"u8, (NamedFlagsType.A | NamedFlagsType.B | NamedFlagsType.C).AsUtf8Span());
		Assert.Equal("Value B, Value D"u8, (NamedFlagsType.B | NamedFlagsType.D).AsUtf8Span());
		Assert.Equal("Value A, Value B, Value C, Value D, Value E"u8, (NamedFlagsType.A | NamedFlagsType.B | NamedFlagsType.C | NamedFlagsType.D | NamedFlagsType.E).AsUtf8Span());
	}

	[Fact]
	public void FromStringFastReturnsCorrectResult()
	{
		Assert.Equal(IntegerType.Byte, IntegerTypeGen.FromStringFast("Byte"));
		Assert.Equal(IntegerType.Short, IntegerTypeGen.FromStringFast("Short"));
		Assert.Equal(IntegerType.Int, IntegerTypeGen.FromStringFast("Int"));
		Assert.Equal(IntegerType.Long, IntegerTypeGen.FromStringFast("Long"));

		Assert.Equal(Language.CSharp, LanguageGen.FromStringFast("C#"));
		Assert.Equal(Language.CPlusPlus, LanguageGen.FromStringFast("C++"));

		Assert.Equal(DayOfWeek.Sunday, DayOfWeekGen.FromStringFast("Sunday"));
		Assert.Equal(DateTimeKind.Unspecified, DateTimeKindGen.FromStringFast("Unspecified"));
		Assert.Throws<ArgumentOutOfRangeException>(() => DateTimeKindGen.FromStringFast("Invalid"));
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
	public void ContainsDefinedFlagsOnlyReturnsCorrectResult()
	{
		// Test with FlagsType enum (1, 2, 4, 8, 16)
		Assert.True(FlagsType.None.ContainsDefinedFlagsOnly());
		Assert.True(FlagsType.A.ContainsDefinedFlagsOnly());
		Assert.True(FlagsType.B.ContainsDefinedFlagsOnly());
		Assert.True(FlagsType.C.ContainsDefinedFlagsOnly());
		Assert.True(FlagsType.D.ContainsDefinedFlagsOnly());
		Assert.True(FlagsType.E.ContainsDefinedFlagsOnly());

		// Test with valid combinations
		Assert.True((FlagsType.A | FlagsType.B).ContainsDefinedFlagsOnly());
		Assert.True((FlagsType.A | FlagsType.B | FlagsType.C).ContainsDefinedFlagsOnly());
		Assert.True((FlagsType.B | FlagsType.D).ContainsDefinedFlagsOnly());
		Assert.True((FlagsType.A | FlagsType.B | FlagsType.C | FlagsType.D | FlagsType.E).ContainsDefinedFlagsOnly());

		// Test with undefined flags
		Assert.False(((FlagsType)32).ContainsDefinedFlagsOnly());
		Assert.False(((FlagsType)33).ContainsDefinedFlagsOnly());
		Assert.False(((FlagsType)63).ContainsDefinedFlagsOnly());

		// Test with FlagsTypeWithMissingBits enum (0, 1, 2, 8, 16)
		Assert.True(FlagsTypeWithMissingBits.None.ContainsDefinedFlagsOnly());
		Assert.True(FlagsTypeWithMissingBits.A.ContainsDefinedFlagsOnly());
		Assert.True(FlagsTypeWithMissingBits.B.ContainsDefinedFlagsOnly());
		Assert.True(FlagsTypeWithMissingBits.D.ContainsDefinedFlagsOnly());
		Assert.True(FlagsTypeWithMissingBits.E.ContainsDefinedFlagsOnly());

		// Test with valid combinations for FlagsTypeWithMissingBits
		Assert.True((FlagsTypeWithMissingBits.A | FlagsTypeWithMissingBits.B).ContainsDefinedFlagsOnly());
		Assert.True((FlagsTypeWithMissingBits.B | FlagsTypeWithMissingBits.D).ContainsDefinedFlagsOnly());

		// Test with undefined flags for FlagsTypeWithMissingBits
		Assert.False(((FlagsTypeWithMissingBits)4).ContainsDefinedFlagsOnly());
		Assert.False(((FlagsTypeWithMissingBits)5).ContainsDefinedFlagsOnly());
		Assert.False(((FlagsTypeWithMissingBits)6).ContainsDefinedFlagsOnly());
		Assert.False(((FlagsTypeWithMissingBits)7).ContainsDefinedFlagsOnly());
		Assert.False(((FlagsTypeWithMissingBits)31).ContainsDefinedFlagsOnly());
		Assert.False(((FlagsTypeWithMissingBits)32).ContainsDefinedFlagsOnly());
		Assert.False(((FlagsTypeWithMissingBits)33).ContainsDefinedFlagsOnly());

		// Test with NamedFlagsType enum
		Assert.True(NamedFlagsType.None.ContainsDefinedFlagsOnly());
		Assert.True(NamedFlagsType.A.ContainsDefinedFlagsOnly());
		Assert.True(NamedFlagsType.B.ContainsDefinedFlagsOnly());
		Assert.True(NamedFlagsType.C.ContainsDefinedFlagsOnly());
		Assert.True(NamedFlagsType.D.ContainsDefinedFlagsOnly());
		Assert.True(NamedFlagsType.E.ContainsDefinedFlagsOnly());

		// Test with valid combinations for NamedFlagsType
		Assert.True((NamedFlagsType.A | NamedFlagsType.B).ContainsDefinedFlagsOnly());
		Assert.True((NamedFlagsType.A | NamedFlagsType.B | NamedFlagsType.C).ContainsDefinedFlagsOnly());
		Assert.True((NamedFlagsType.B | NamedFlagsType.D).ContainsDefinedFlagsOnly());
		Assert.True((NamedFlagsType.A | NamedFlagsType.B | NamedFlagsType.C | NamedFlagsType.D | NamedFlagsType.E).ContainsDefinedFlagsOnly());

		// Test with undefined flags for NamedFlagsType (assuming it uses the same values as FlagsType)
		Assert.False(((NamedFlagsType)32).ContainsDefinedFlagsOnly());
		Assert.False(((NamedFlagsType)33).ContainsDefinedFlagsOnly());
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

	[Fact]
	public void BinarySerializationWorksCorrectly()
	{
		using MemoryStream ms = new();
		using BinaryWriter writer = new(ms);
		using BinaryReader reader = new(ms);

		RoundTrip<byte, UnderlyingTypedByte>(writer, reader, UnderlyingTypedByte.A, UnderlyingTypedByteGen.Write, UnderlyingTypedByteGen.ReadUnderlyingTypedByte);
		RoundTrip<ushort, UnderlyingTypedUShort>(writer, reader, UnderlyingTypedUShort.A, UnderlyingTypedUShortGen.Write, UnderlyingTypedUShortGen.ReadUnderlyingTypedUShort);
		RoundTrip<uint, UnderlyingTypedUInt>(writer, reader, UnderlyingTypedUInt.A, UnderlyingTypedUIntGen.Write, UnderlyingTypedUIntGen.ReadUnderlyingTypedUInt);
		RoundTrip<ulong, UnderlyingTypedULong>(writer, reader, UnderlyingTypedULong.A, UnderlyingTypedULongGen.Write, UnderlyingTypedULongGen.ReadUnderlyingTypedULong);
		RoundTrip<sbyte, UnderlyingTypedSByte>(writer, reader, UnderlyingTypedSByte.A, UnderlyingTypedSByteGen.Write, UnderlyingTypedSByteGen.ReadUnderlyingTypedSByte);
		RoundTrip<short, UnderlyingTypedShort>(writer, reader, UnderlyingTypedShort.A, UnderlyingTypedShortGen.Write, UnderlyingTypedShortGen.ReadUnderlyingTypedShort);
		RoundTrip<int, UnderlyingTypedInt>(writer, reader, UnderlyingTypedInt.A, UnderlyingTypedIntGen.Write, UnderlyingTypedIntGen.ReadUnderlyingTypedInt);
		RoundTrip<long, UnderlyingTypedLong>(writer, reader, UnderlyingTypedLong.A, UnderlyingTypedLongGen.Write, UnderlyingTypedLongGen.ReadUnderlyingTypedLong);
	}

	private static unsafe void RoundTrip<TPrimitive, TEnum>(
		BinaryWriter writer,
		BinaryReader reader,
		TEnum value,
		Action<BinaryWriter, TEnum> writeFunc,
		Func<BinaryReader, TEnum> readFunc)
		where TPrimitive : unmanaged
		where TEnum : struct, Enum
	{
		writer.BaseStream.Position = 0;
		writeFunc(writer, value);
		Assert.Equal(sizeof(TPrimitive), writer.BaseStream.Position);

		writer.BaseStream.Position = 0;
		TEnum result = readFunc(reader);
		Assert.Equal(sizeof(TPrimitive), writer.BaseStream.Position);

		Assert.Equal(value, result);
	}

	[Fact]
	public void IsDefinedReturnsCorrectResult()
	{
		Assert.True(IntegerType.Byte.IsDefined());
		Assert.True(IntegerType.Short.IsDefined());
		Assert.True(IntegerType.Int.IsDefined());
		Assert.True(IntegerType.Long.IsDefined());
		Assert.False(((IntegerType)4).IsDefined());

		Assert.True(UnderlyingTypedByte.A.IsDefined());
		Assert.True(UnderlyingTypedByte.B.IsDefined());
		Assert.True(UnderlyingTypedByte.C.IsDefined());
		Assert.False(((UnderlyingTypedByte)3).IsDefined());

		Assert.True(UnderlyingTypedUShort.A.IsDefined());
		Assert.True(UnderlyingTypedUShort.B.IsDefined());
		Assert.True(UnderlyingTypedUShort.C.IsDefined());
		Assert.False(((UnderlyingTypedUShort)3).IsDefined());

		Assert.True(UnderlyingTypedUInt.A.IsDefined());
		Assert.True(UnderlyingTypedUInt.B.IsDefined());
		Assert.True(UnderlyingTypedUInt.C.IsDefined());
		Assert.False(((UnderlyingTypedUInt)3).IsDefined());

		Assert.True(UnderlyingTypedULong.A.IsDefined());
		Assert.True(UnderlyingTypedULong.B.IsDefined());
		Assert.True(UnderlyingTypedULong.C.IsDefined());
		Assert.False(((UnderlyingTypedULong)3).IsDefined());

		Assert.True(UnderlyingTypedSByte.A.IsDefined());
		Assert.True(UnderlyingTypedSByte.B.IsDefined());
		Assert.True(UnderlyingTypedSByte.C.IsDefined());
		Assert.False(((UnderlyingTypedSByte)3).IsDefined());

		Assert.True(UnderlyingTypedShort.A.IsDefined());
		Assert.True(UnderlyingTypedShort.B.IsDefined());
		Assert.True(UnderlyingTypedShort.C.IsDefined());
		Assert.False(((UnderlyingTypedShort)3).IsDefined());

		Assert.True(UnderlyingTypedInt.A.IsDefined());
		Assert.True(UnderlyingTypedInt.B.IsDefined());
		Assert.True(UnderlyingTypedInt.C.IsDefined());
		Assert.False(((UnderlyingTypedInt)3).IsDefined());

		Assert.True(UnderlyingTypedLong.A.IsDefined());
		Assert.True(UnderlyingTypedLong.B.IsDefined());
		Assert.True(UnderlyingTypedLong.C.IsDefined());
		Assert.False(((UnderlyingTypedLong)3).IsDefined());
	}
}
