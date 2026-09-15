using EnumGenerator.Tests.Integration.Enums;
using TUnit.Assertions.Enums;

namespace EnumGenerator.Tests.Integration;

public sealed class EnumTests
{
	[Test]
	public async Task Values()
	{
		await Assert.That(IntegerTypeGen.Values.Count).IsEqualTo(4);
		await Assert.That(IntegerTypeGen.Values[0]).IsEqualTo(IntegerType.Byte);
		await Assert.That(IntegerTypeGen.Values[1]).IsEqualTo(IntegerType.Short);
		await Assert.That(IntegerTypeGen.Values[2]).IsEqualTo(IntegerType.Int);
		await Assert.That(IntegerTypeGen.Values[3]).IsEqualTo(IntegerType.Long);
	}

	[Test]
	public async Task NullTerminatedMemberNames()
	{
		await Assert.That("Byte\0Short\0Int\0Long\0"u8.SequenceEqual(IntegerTypeGen.NullTerminatedMemberNames)).IsTrue();
	}

	[Test]
	public async Task ToStringFastReturnsCorrectResult()
	{
		await Assert.That(IntegerType.Byte.ToStringFast()).IsEqualTo("Byte");
		await Assert.That(IntegerType.Short.ToStringFast()).IsEqualTo("Short");
		await Assert.That(IntegerType.Int.ToStringFast()).IsEqualTo("Int");
		await Assert.That(IntegerType.Long.ToStringFast()).IsEqualTo("Long");

		await Assert.That(Language.CSharp.ToStringFast()).IsEqualTo("C#");
		await Assert.That(Language.CPlusPlus.ToStringFast()).IsEqualTo("C++");

		await Assert.That(DayOfWeek.Sunday.ToStringFast()).IsEqualTo("Sunday");
		await Assert.That(DateTimeKind.Unspecified.ToStringFast()).IsEqualTo("Unspecified");
		await Assert.That(() => ((DateTimeKind)3).ToStringFast()).ThrowsExactly<ArgumentOutOfRangeException>();

		await Assert.That(FlagsType.None.ToStringFast()).IsEqualTo("None");
		await Assert.That(FlagsType.A.ToStringFast()).IsEqualTo("A");
		await Assert.That(FlagsType.B.ToStringFast()).IsEqualTo("B");
		await Assert.That(FlagsType.C.ToStringFast()).IsEqualTo("C");
		await Assert.That(FlagsType.D.ToStringFast()).IsEqualTo("D");
		await Assert.That(FlagsType.E.ToStringFast()).IsEqualTo("E");

		await Assert.That((FlagsType.None | FlagsType.A | FlagsType.B).ToStringFast()).IsEqualTo("A, B");
		await Assert.That((FlagsType.A | FlagsType.B).ToStringFast()).IsEqualTo("A, B");
		await Assert.That((FlagsType.A | FlagsType.B | FlagsType.C).ToStringFast()).IsEqualTo("A, B, C");
		await Assert.That((FlagsType.B | FlagsType.D).ToStringFast()).IsEqualTo("B, D");
		await Assert.That((FlagsType.A | FlagsType.B | FlagsType.C | FlagsType.D | FlagsType.E).ToStringFast()).IsEqualTo("A, B, C, D, E");
		await Assert.That(() => ((FlagsType)32).ToStringFast()).ThrowsExactly<ArgumentOutOfRangeException>();
		await Assert.That(() => ((FlagsType)33).ToStringFast()).ThrowsExactly<ArgumentOutOfRangeException>();

		await Assert.That(FlagsTypeWithMissingBits.None.ToStringFast()).IsEqualTo("None");
		await Assert.That(FlagsTypeWithMissingBits.A.ToStringFast()).IsEqualTo("A");
		await Assert.That(FlagsTypeWithMissingBits.B.ToStringFast()).IsEqualTo("B");
		await Assert.That(() => ((FlagsTypeWithMissingBits)4).ToStringFast()).ThrowsExactly<ArgumentOutOfRangeException>();
		await Assert.That(() => ((FlagsTypeWithMissingBits)5).ToStringFast()).ThrowsExactly<ArgumentOutOfRangeException>();
		await Assert.That(() => ((FlagsTypeWithMissingBits)6).ToStringFast()).ThrowsExactly<ArgumentOutOfRangeException>();
		await Assert.That(() => ((FlagsTypeWithMissingBits)7).ToStringFast()).ThrowsExactly<ArgumentOutOfRangeException>();
		await Assert.That(FlagsTypeWithMissingBits.D.ToStringFast()).IsEqualTo("D");
		await Assert.That(FlagsTypeWithMissingBits.E.ToStringFast()).IsEqualTo("E");

		await Assert.That((FlagsTypeWithMissingBits.None | FlagsTypeWithMissingBits.A | FlagsTypeWithMissingBits.B).ToStringFast()).IsEqualTo("A, B");
		await Assert.That((FlagsTypeWithMissingBits.A | FlagsTypeWithMissingBits.B).ToStringFast()).IsEqualTo("A, B");
		await Assert.That((FlagsTypeWithMissingBits.B | FlagsTypeWithMissingBits.D).ToStringFast()).IsEqualTo("B, D");
		await Assert.That(() => ((FlagsTypeWithMissingBits)31).ToStringFast()).ThrowsExactly<ArgumentOutOfRangeException>();
		await Assert.That(() => ((FlagsTypeWithMissingBits)32).ToStringFast()).ThrowsExactly<ArgumentOutOfRangeException>();
		await Assert.That(() => ((FlagsTypeWithMissingBits)33).ToStringFast()).ThrowsExactly<ArgumentOutOfRangeException>();

		await Assert.That(NamedFlagsType.None.ToStringFast()).IsEqualTo("No value");
		await Assert.That(NamedFlagsType.A.ToStringFast()).IsEqualTo("Value A");
		await Assert.That(NamedFlagsType.B.ToStringFast()).IsEqualTo("Value B");
		await Assert.That(NamedFlagsType.C.ToStringFast()).IsEqualTo("Value C");
		await Assert.That(NamedFlagsType.D.ToStringFast()).IsEqualTo("Value D");
		await Assert.That(NamedFlagsType.E.ToStringFast()).IsEqualTo("Value E");

		await Assert.That((NamedFlagsType.None | NamedFlagsType.A | NamedFlagsType.B).ToStringFast()).IsEqualTo("Value A, Value B");
		await Assert.That((NamedFlagsType.A | NamedFlagsType.B).ToStringFast()).IsEqualTo("Value A, Value B");
		await Assert.That((NamedFlagsType.A | NamedFlagsType.B | NamedFlagsType.C).ToStringFast()).IsEqualTo("Value A, Value B, Value C");
		await Assert.That((NamedFlagsType.B | NamedFlagsType.D).ToStringFast()).IsEqualTo("Value B, Value D");
		await Assert.That((NamedFlagsType.A | NamedFlagsType.B | NamedFlagsType.C | NamedFlagsType.D | NamedFlagsType.E).ToStringFast()).IsEqualTo("Value A, Value B, Value C, Value D, Value E");
	}

	[Test]
	public async Task AsUtf8SpanReturnsCorrectResult()
	{
		await Assert.That("Byte"u8.SequenceEqual(IntegerType.Byte.AsUtf8Span())).IsTrue();
		await Assert.That("Short"u8.SequenceEqual(IntegerType.Short.AsUtf8Span())).IsTrue();
		await Assert.That("Int"u8.SequenceEqual(IntegerType.Int.AsUtf8Span())).IsTrue();
		await Assert.That("Long"u8.SequenceEqual(IntegerType.Long.AsUtf8Span())).IsTrue();

		await Assert.That("C#"u8.SequenceEqual(Language.CSharp.AsUtf8Span())).IsTrue();
		await Assert.That("C++"u8.SequenceEqual(Language.CPlusPlus.AsUtf8Span())).IsTrue();

		await Assert.That("Sunday"u8.SequenceEqual(DayOfWeek.Sunday.AsUtf8Span())).IsTrue();
		await Assert.That("Unspecified"u8.SequenceEqual(DateTimeKind.Unspecified.AsUtf8Span())).IsTrue();
		await Assert.That(() => { _ = ((DateTimeKind)3).AsUtf8Span(); }).ThrowsExactly<ArgumentOutOfRangeException>();

		await Assert.That(FlagsType.None.AsUtf8Span().ToArray()).IsEquivalentTo("None"u8.ToArray(), CollectionOrdering.Matching);
		await Assert.That(FlagsType.A.AsUtf8Span().ToArray()).IsEquivalentTo("A"u8.ToArray(), CollectionOrdering.Matching);
		await Assert.That(FlagsType.B.AsUtf8Span().ToArray()).IsEquivalentTo("B"u8.ToArray(), CollectionOrdering.Matching);
		await Assert.That(FlagsType.C.AsUtf8Span().ToArray()).IsEquivalentTo("C"u8.ToArray(), CollectionOrdering.Matching);
		await Assert.That(FlagsType.D.AsUtf8Span().ToArray()).IsEquivalentTo("D"u8.ToArray(), CollectionOrdering.Matching);
		await Assert.That(FlagsType.E.AsUtf8Span().ToArray()).IsEquivalentTo("E"u8.ToArray(), CollectionOrdering.Matching);

		await Assert.That((FlagsType.None | FlagsType.A | FlagsType.B).AsUtf8Span().ToArray()).IsEquivalentTo("A, B"u8.ToArray(), CollectionOrdering.Matching);
		await Assert.That((FlagsType.A | FlagsType.B).AsUtf8Span().ToArray()).IsEquivalentTo("A, B"u8.ToArray(), CollectionOrdering.Matching);
		await Assert.That((FlagsType.A | FlagsType.B | FlagsType.C).AsUtf8Span().ToArray()).IsEquivalentTo("A, B, C"u8.ToArray(), CollectionOrdering.Matching);
		await Assert.That((FlagsType.B | FlagsType.D).AsUtf8Span().ToArray()).IsEquivalentTo("B, D"u8.ToArray(), CollectionOrdering.Matching);
		await Assert.That((FlagsType.A | FlagsType.B | FlagsType.C | FlagsType.D | FlagsType.E).AsUtf8Span().ToArray()).IsEquivalentTo("A, B, C, D, E"u8.ToArray(), CollectionOrdering.Matching);
		await Assert.That(() => { _ = ((FlagsType)32).AsUtf8Span(); }).ThrowsExactly<ArgumentOutOfRangeException>();
		await Assert.That(() => { _ = ((FlagsType)33).AsUtf8Span(); }).ThrowsExactly<ArgumentOutOfRangeException>();

		await Assert.That(FlagsTypeWithMissingBits.None.AsUtf8Span().ToArray()).IsEquivalentTo("None"u8.ToArray(), CollectionOrdering.Matching);
		await Assert.That(FlagsTypeWithMissingBits.A.AsUtf8Span().ToArray()).IsEquivalentTo("A"u8.ToArray(), CollectionOrdering.Matching);
		await Assert.That(FlagsTypeWithMissingBits.B.AsUtf8Span().ToArray()).IsEquivalentTo("B"u8.ToArray(), CollectionOrdering.Matching);
		await Assert.That(() => { _ = ((FlagsTypeWithMissingBits)4).AsUtf8Span(); }).ThrowsExactly<ArgumentOutOfRangeException>();
		await Assert.That(() => { _ = ((FlagsTypeWithMissingBits)5).AsUtf8Span(); }).ThrowsExactly<ArgumentOutOfRangeException>();
		await Assert.That(() => { _ = ((FlagsTypeWithMissingBits)6).AsUtf8Span(); }).ThrowsExactly<ArgumentOutOfRangeException>();
		await Assert.That(() => { _ = ((FlagsTypeWithMissingBits)7).AsUtf8Span(); }).ThrowsExactly<ArgumentOutOfRangeException>();
		await Assert.That(FlagsTypeWithMissingBits.D.AsUtf8Span().ToArray()).IsEquivalentTo("D"u8.ToArray(), CollectionOrdering.Matching);
		await Assert.That(FlagsTypeWithMissingBits.E.AsUtf8Span().ToArray()).IsEquivalentTo("E"u8.ToArray(), CollectionOrdering.Matching);

		await Assert.That((FlagsTypeWithMissingBits.None | FlagsTypeWithMissingBits.A | FlagsTypeWithMissingBits.B).AsUtf8Span().ToArray()).IsEquivalentTo("A, B"u8.ToArray(), CollectionOrdering.Matching);
		await Assert.That((FlagsTypeWithMissingBits.A | FlagsTypeWithMissingBits.B).AsUtf8Span().ToArray()).IsEquivalentTo("A, B"u8.ToArray(), CollectionOrdering.Matching);
		await Assert.That((FlagsTypeWithMissingBits.B | FlagsTypeWithMissingBits.D).AsUtf8Span().ToArray()).IsEquivalentTo("B, D"u8.ToArray(), CollectionOrdering.Matching);
		await Assert.That(() => { _ = ((FlagsTypeWithMissingBits)31).AsUtf8Span(); }).ThrowsExactly<ArgumentOutOfRangeException>();
		await Assert.That(() => { _ = ((FlagsTypeWithMissingBits)32).AsUtf8Span(); }).ThrowsExactly<ArgumentOutOfRangeException>();
		await Assert.That(() => { _ = ((FlagsTypeWithMissingBits)33).AsUtf8Span(); }).ThrowsExactly<ArgumentOutOfRangeException>();

		await Assert.That(NamedFlagsType.None.AsUtf8Span().ToArray()).IsEquivalentTo("No value"u8.ToArray(), CollectionOrdering.Matching);
		await Assert.That(NamedFlagsType.A.AsUtf8Span().ToArray()).IsEquivalentTo("Value A"u8.ToArray(), CollectionOrdering.Matching);
		await Assert.That(NamedFlagsType.B.AsUtf8Span().ToArray()).IsEquivalentTo("Value B"u8.ToArray(), CollectionOrdering.Matching);
		await Assert.That(NamedFlagsType.C.AsUtf8Span().ToArray()).IsEquivalentTo("Value C"u8.ToArray(), CollectionOrdering.Matching);
		await Assert.That(NamedFlagsType.D.AsUtf8Span().ToArray()).IsEquivalentTo("Value D"u8.ToArray(), CollectionOrdering.Matching);
		await Assert.That(NamedFlagsType.E.AsUtf8Span().ToArray()).IsEquivalentTo("Value E"u8.ToArray(), CollectionOrdering.Matching);

		await Assert.That((NamedFlagsType.None | NamedFlagsType.A | NamedFlagsType.B).AsUtf8Span().ToArray()).IsEquivalentTo("Value A, Value B"u8.ToArray(), CollectionOrdering.Matching);
		await Assert.That((NamedFlagsType.A | NamedFlagsType.B).AsUtf8Span().ToArray()).IsEquivalentTo("Value A, Value B"u8.ToArray(), CollectionOrdering.Matching);
		await Assert.That((NamedFlagsType.A | NamedFlagsType.B | NamedFlagsType.C).AsUtf8Span().ToArray()).IsEquivalentTo("Value A, Value B, Value C"u8.ToArray(), CollectionOrdering.Matching);
		await Assert.That((NamedFlagsType.B | NamedFlagsType.D).AsUtf8Span().ToArray()).IsEquivalentTo("Value B, Value D"u8.ToArray(), CollectionOrdering.Matching);
		await Assert.That((NamedFlagsType.A | NamedFlagsType.B | NamedFlagsType.C | NamedFlagsType.D | NamedFlagsType.E).AsUtf8Span().ToArray()).IsEquivalentTo("Value A, Value B, Value C, Value D, Value E"u8.ToArray(), CollectionOrdering.Matching);
	}

	[Test]
	public async Task FromStringFastReturnsCorrectResult()
	{
		await Assert.That(IntegerTypeGen.FromStringFast("Byte")).IsEqualTo(IntegerType.Byte);
		await Assert.That(IntegerTypeGen.FromStringFast("Short")).IsEqualTo(IntegerType.Short);
		await Assert.That(IntegerTypeGen.FromStringFast("Int")).IsEqualTo(IntegerType.Int);
		await Assert.That(IntegerTypeGen.FromStringFast("Long")).IsEqualTo(IntegerType.Long);

		await Assert.That(LanguageGen.FromStringFast("C#")).IsEqualTo(Language.CSharp);
		await Assert.That(LanguageGen.FromStringFast("C++")).IsEqualTo(Language.CPlusPlus);

		await Assert.That(DayOfWeekGen.FromStringFast("Sunday")).IsEqualTo(DayOfWeek.Sunday);
		await Assert.That(DateTimeKindGen.FromStringFast("Unspecified")).IsEqualTo(DateTimeKind.Unspecified);
		await Assert.That(() => DateTimeKindGen.FromStringFast("Invalid")).ThrowsExactly<ArgumentOutOfRangeException>();
	}

	[Test]
	public async Task HasFlagFastReturnsCorrectResult()
	{
		const FlagsType ab = FlagsType.A | FlagsType.B;
		await Assert.That(ab.HasFlagFast(FlagsType.A)).IsTrue();
		await Assert.That(ab.HasFlagFast(FlagsType.B)).IsTrue();
		await Assert.That(ab.HasFlagFast(FlagsType.C)).IsFalse();
		await Assert.That(ab.HasFlagFast(FlagsType.D)).IsFalse();
		await Assert.That(ab.HasFlagFast(FlagsType.E)).IsFalse();
	}

	[Test]
	public async Task GetIndexReturnsCorrectResult()
	{
		await Assert.That(IntegerType.Byte.GetIndex()).IsEqualTo(0);
		await Assert.That(IntegerType.Short.GetIndex()).IsEqualTo(1);
		await Assert.That(IntegerType.Int.GetIndex()).IsEqualTo(2);
		await Assert.That(IntegerType.Long.GetIndex()).IsEqualTo(3);
	}

	[Test]
	public async Task GetIndexFlagReturnsCorrectResult()
	{
		await Assert.That(FlagsType.None.GetIndex()).IsEqualTo(0);
		await Assert.That(FlagsType.A.GetIndex()).IsEqualTo(1);
		await Assert.That(FlagsType.B.GetIndex()).IsEqualTo(2);
		await Assert.That(FlagsType.C.GetIndex()).IsEqualTo(3);
		await Assert.That(FlagsType.D.GetIndex()).IsEqualTo(4);
		await Assert.That(FlagsType.E.GetIndex()).IsEqualTo(5);
	}

	[Test]
	public async Task FromIndexReturnsCorrectResult()
	{
		await Assert.That(IntegerTypeGen.FromIndex(0)).IsEqualTo(IntegerType.Byte);
		await Assert.That(IntegerTypeGen.FromIndex(1)).IsEqualTo(IntegerType.Short);
		await Assert.That(IntegerTypeGen.FromIndex(2)).IsEqualTo(IntegerType.Int);
		await Assert.That(IntegerTypeGen.FromIndex(3)).IsEqualTo(IntegerType.Long);
	}

	[Test]
	public async Task ContainsDefinedFlagsOnlyReturnsCorrectResult()
	{
		// Test with FlagsType enum (1, 2, 4, 8, 16)
		await Assert.That(FlagsType.None.ContainsDefinedFlagsOnly()).IsTrue();
		await Assert.That(FlagsType.A.ContainsDefinedFlagsOnly()).IsTrue();
		await Assert.That(FlagsType.B.ContainsDefinedFlagsOnly()).IsTrue();
		await Assert.That(FlagsType.C.ContainsDefinedFlagsOnly()).IsTrue();
		await Assert.That(FlagsType.D.ContainsDefinedFlagsOnly()).IsTrue();
		await Assert.That(FlagsType.E.ContainsDefinedFlagsOnly()).IsTrue();

		// Test with valid combinations
		await Assert.That((FlagsType.A | FlagsType.B).ContainsDefinedFlagsOnly()).IsTrue();
		await Assert.That((FlagsType.A | FlagsType.B | FlagsType.C).ContainsDefinedFlagsOnly()).IsTrue();
		await Assert.That((FlagsType.B | FlagsType.D).ContainsDefinedFlagsOnly()).IsTrue();
		await Assert.That((FlagsType.A | FlagsType.B | FlagsType.C | FlagsType.D | FlagsType.E).ContainsDefinedFlagsOnly()).IsTrue();

		// Test with undefined flags
		await Assert.That(((FlagsType)32).ContainsDefinedFlagsOnly()).IsFalse();
		await Assert.That(((FlagsType)33).ContainsDefinedFlagsOnly()).IsFalse();
		await Assert.That(((FlagsType)63).ContainsDefinedFlagsOnly()).IsFalse();

		// Test with FlagsTypeWithMissingBits enum (0, 1, 2, 8, 16)
		await Assert.That(FlagsTypeWithMissingBits.None.ContainsDefinedFlagsOnly()).IsTrue();
		await Assert.That(FlagsTypeWithMissingBits.A.ContainsDefinedFlagsOnly()).IsTrue();
		await Assert.That(FlagsTypeWithMissingBits.B.ContainsDefinedFlagsOnly()).IsTrue();
		await Assert.That(FlagsTypeWithMissingBits.D.ContainsDefinedFlagsOnly()).IsTrue();
		await Assert.That(FlagsTypeWithMissingBits.E.ContainsDefinedFlagsOnly()).IsTrue();

		// Test with valid combinations for FlagsTypeWithMissingBits
		await Assert.That((FlagsTypeWithMissingBits.A | FlagsTypeWithMissingBits.B).ContainsDefinedFlagsOnly()).IsTrue();
		await Assert.That((FlagsTypeWithMissingBits.B | FlagsTypeWithMissingBits.D).ContainsDefinedFlagsOnly()).IsTrue();

		// Test with undefined flags for FlagsTypeWithMissingBits
		await Assert.That(((FlagsTypeWithMissingBits)4).ContainsDefinedFlagsOnly()).IsFalse();
		await Assert.That(((FlagsTypeWithMissingBits)5).ContainsDefinedFlagsOnly()).IsFalse();
		await Assert.That(((FlagsTypeWithMissingBits)6).ContainsDefinedFlagsOnly()).IsFalse();
		await Assert.That(((FlagsTypeWithMissingBits)7).ContainsDefinedFlagsOnly()).IsFalse();
		await Assert.That(((FlagsTypeWithMissingBits)31).ContainsDefinedFlagsOnly()).IsFalse();
		await Assert.That(((FlagsTypeWithMissingBits)32).ContainsDefinedFlagsOnly()).IsFalse();
		await Assert.That(((FlagsTypeWithMissingBits)33).ContainsDefinedFlagsOnly()).IsFalse();

		// Test with NamedFlagsType enum
		await Assert.That(NamedFlagsType.None.ContainsDefinedFlagsOnly()).IsTrue();
		await Assert.That(NamedFlagsType.A.ContainsDefinedFlagsOnly()).IsTrue();
		await Assert.That(NamedFlagsType.B.ContainsDefinedFlagsOnly()).IsTrue();
		await Assert.That(NamedFlagsType.C.ContainsDefinedFlagsOnly()).IsTrue();
		await Assert.That(NamedFlagsType.D.ContainsDefinedFlagsOnly()).IsTrue();
		await Assert.That(NamedFlagsType.E.ContainsDefinedFlagsOnly()).IsTrue();

		// Test with valid combinations for NamedFlagsType
		await Assert.That((NamedFlagsType.A | NamedFlagsType.B).ContainsDefinedFlagsOnly()).IsTrue();
		await Assert.That((NamedFlagsType.A | NamedFlagsType.B | NamedFlagsType.C).ContainsDefinedFlagsOnly()).IsTrue();
		await Assert.That((NamedFlagsType.B | NamedFlagsType.D).ContainsDefinedFlagsOnly()).IsTrue();
		await Assert.That((NamedFlagsType.A | NamedFlagsType.B | NamedFlagsType.C | NamedFlagsType.D | NamedFlagsType.E).ContainsDefinedFlagsOnly()).IsTrue();

		// Test with undefined flags for NamedFlagsType (assuming it uses the same values as FlagsType)
		await Assert.That(((NamedFlagsType)32).ContainsDefinedFlagsOnly()).IsFalse();
		await Assert.That(((NamedFlagsType)33).ContainsDefinedFlagsOnly()).IsFalse();
	}

	[Test]
	public async Task FromIndexFlagReturnsCorrectResult()
	{
		await Assert.That(FlagsTypeGen.FromIndex(0)).IsEqualTo(FlagsType.None);
		await Assert.That(FlagsTypeGen.FromIndex(1)).IsEqualTo(FlagsType.A);
		await Assert.That(FlagsTypeGen.FromIndex(2)).IsEqualTo(FlagsType.B);
		await Assert.That(FlagsTypeGen.FromIndex(3)).IsEqualTo(FlagsType.C);
		await Assert.That(FlagsTypeGen.FromIndex(4)).IsEqualTo(FlagsType.D);
		await Assert.That(FlagsTypeGen.FromIndex(5)).IsEqualTo(FlagsType.E);
	}

	[Test]
	public async Task BinarySerializationWorksCorrectly()
	{
		using MemoryStream ms = new();
		using BinaryWriter writer = new(ms);
		using BinaryReader reader = new(ms);

		await RoundTrip<byte, UnderlyingTypedByte>(writer, reader, UnderlyingTypedByte.A, UnderlyingTypedByteGen.Write, UnderlyingTypedByteGen.ReadUnderlyingTypedByte);
		await RoundTrip<ushort, UnderlyingTypedUShort>(writer, reader, UnderlyingTypedUShort.A, UnderlyingTypedUShortGen.Write, UnderlyingTypedUShortGen.ReadUnderlyingTypedUShort);
		await RoundTrip<uint, UnderlyingTypedUInt>(writer, reader, UnderlyingTypedUInt.A, UnderlyingTypedUIntGen.Write, UnderlyingTypedUIntGen.ReadUnderlyingTypedUInt);
		await RoundTrip<ulong, UnderlyingTypedULong>(writer, reader, UnderlyingTypedULong.A, UnderlyingTypedULongGen.Write, UnderlyingTypedULongGen.ReadUnderlyingTypedULong);
		await RoundTrip<sbyte, UnderlyingTypedSByte>(writer, reader, UnderlyingTypedSByte.A, UnderlyingTypedSByteGen.Write, UnderlyingTypedSByteGen.ReadUnderlyingTypedSByte);
		await RoundTrip<short, UnderlyingTypedShort>(writer, reader, UnderlyingTypedShort.A, UnderlyingTypedShortGen.Write, UnderlyingTypedShortGen.ReadUnderlyingTypedShort);
		await RoundTrip<int, UnderlyingTypedInt>(writer, reader, UnderlyingTypedInt.A, UnderlyingTypedIntGen.Write, UnderlyingTypedIntGen.ReadUnderlyingTypedInt);
		await RoundTrip<long, UnderlyingTypedLong>(writer, reader, UnderlyingTypedLong.A, UnderlyingTypedLongGen.Write, UnderlyingTypedLongGen.ReadUnderlyingTypedLong);
	}

	private static async Task RoundTrip<TPrimitive, TEnum>(
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
		await Assert.That(writer.BaseStream.Position).IsEqualTo(SizeOf<TPrimitive>());

		writer.BaseStream.Position = 0;
		TEnum result = readFunc(reader);
		await Assert.That(writer.BaseStream.Position).IsEqualTo(SizeOf<TPrimitive>());

		await Assert.That(result).IsEqualTo(value);
	}

	/// <summary>
	/// <c>sizeof(T)</c> on an unmanaged type parameter needs an unsafe context, and `await` is not allowed in
	/// one - so the unsafe bit lives here rather than on the async <see cref="RoundTrip{TPrimitive,TEnum}"/>.
	/// </summary>
	private static unsafe int SizeOf<T>()
		where T : unmanaged
	{
		return sizeof(T);
	}

	[Test]
	public async Task IsDefinedReturnsCorrectResult()
	{
		await Assert.That(IntegerType.Byte.IsDefined()).IsTrue();
		await Assert.That(IntegerType.Short.IsDefined()).IsTrue();
		await Assert.That(IntegerType.Int.IsDefined()).IsTrue();
		await Assert.That(IntegerType.Long.IsDefined()).IsTrue();
		await Assert.That(((IntegerType)4).IsDefined()).IsFalse();

		await Assert.That(UnderlyingTypedByte.A.IsDefined()).IsTrue();
		await Assert.That(UnderlyingTypedByte.B.IsDefined()).IsTrue();
		await Assert.That(UnderlyingTypedByte.C.IsDefined()).IsTrue();
		await Assert.That(((UnderlyingTypedByte)3).IsDefined()).IsFalse();

		await Assert.That(UnderlyingTypedUShort.A.IsDefined()).IsTrue();
		await Assert.That(UnderlyingTypedUShort.B.IsDefined()).IsTrue();
		await Assert.That(UnderlyingTypedUShort.C.IsDefined()).IsTrue();
		await Assert.That(((UnderlyingTypedUShort)3).IsDefined()).IsFalse();

		await Assert.That(UnderlyingTypedUInt.A.IsDefined()).IsTrue();
		await Assert.That(UnderlyingTypedUInt.B.IsDefined()).IsTrue();
		await Assert.That(UnderlyingTypedUInt.C.IsDefined()).IsTrue();
		await Assert.That(((UnderlyingTypedUInt)3).IsDefined()).IsFalse();

		await Assert.That(UnderlyingTypedULong.A.IsDefined()).IsTrue();
		await Assert.That(UnderlyingTypedULong.B.IsDefined()).IsTrue();
		await Assert.That(UnderlyingTypedULong.C.IsDefined()).IsTrue();
		await Assert.That(((UnderlyingTypedULong)3).IsDefined()).IsFalse();

		await Assert.That(UnderlyingTypedSByte.A.IsDefined()).IsTrue();
		await Assert.That(UnderlyingTypedSByte.B.IsDefined()).IsTrue();
		await Assert.That(UnderlyingTypedSByte.C.IsDefined()).IsTrue();
		await Assert.That(((UnderlyingTypedSByte)3).IsDefined()).IsFalse();

		await Assert.That(UnderlyingTypedShort.A.IsDefined()).IsTrue();
		await Assert.That(UnderlyingTypedShort.B.IsDefined()).IsTrue();
		await Assert.That(UnderlyingTypedShort.C.IsDefined()).IsTrue();
		await Assert.That(((UnderlyingTypedShort)3).IsDefined()).IsFalse();

		await Assert.That(UnderlyingTypedInt.A.IsDefined()).IsTrue();
		await Assert.That(UnderlyingTypedInt.B.IsDefined()).IsTrue();
		await Assert.That(UnderlyingTypedInt.C.IsDefined()).IsTrue();
		await Assert.That(((UnderlyingTypedInt)3).IsDefined()).IsFalse();

		await Assert.That(UnderlyingTypedLong.A.IsDefined()).IsTrue();
		await Assert.That(UnderlyingTypedLong.B.IsDefined()).IsTrue();
		await Assert.That(UnderlyingTypedLong.C.IsDefined()).IsTrue();
		await Assert.That(((UnderlyingTypedLong)3).IsDefined()).IsFalse();
	}
}
