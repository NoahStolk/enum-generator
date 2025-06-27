using Xunit;

namespace EnumGenerator.Tests.Integration;

public sealed class EnumAllocationTests
{
	// Declare private enums so that other tests won't call them.
	// Otherwise, they may be initialized by other tests, which would result in premature caching.
	// We want to make sure all caches are empty when these tests start.
	// Note that private enums won't work because the generated code won't be able to access them.
	[GenerateEnumUtilities]
	internal enum Test
	{
		A,
		B,
		C,
		D,
	}

	[GenerateEnumUtilities]
	[Flags]
	internal enum FlagsTest
	{
		None = 0,
		A = 1,
		B = 2,
		C = 4,
		D = 8,
	}

	[Fact]
	public void ToStringLikeMethodsDoNotAllocate()
	{
		long allocatedBytes = GC.GetAllocatedBytesForCurrentThread();

		string a = Test.A.ToStringFast();
		string b = Test.B.ToStringFast();
		ReadOnlySpan<byte> c = Test.C.AsUtf8Span();
		ReadOnlySpan<byte> d = Test.D.AsUtf8Span();

		Assert.Equal(allocatedBytes, GC.GetAllocatedBytesForCurrentThread());

		// Assert to prevent code from being optimized out.
		Assert.Equal("A", a);
		Assert.Equal("B", b);
		Assert.True("C"u8.SequenceEqual(c));
		Assert.True("D"u8.SequenceEqual(d));

		// Flag methods may allocate once.
		string ab = (FlagsTest.A | FlagsTest.B).ToStringFast();
		ReadOnlySpan<byte> cd = (FlagsTest.C | FlagsTest.D).AsUtf8Span();

		allocatedBytes = GC.GetAllocatedBytesForCurrentThread();

		// They shouldn't allocate again.
		string ab2 = string.Empty;
		ReadOnlySpan<byte> cd2 = default;
		for (int i = 0; i < 50; i++)
		{
			ab2 = (FlagsTest.A | FlagsTest.B).ToStringFast();
			cd2 = (FlagsTest.C | FlagsTest.D).AsUtf8Span();
		}

		Assert.Equal(allocatedBytes, GC.GetAllocatedBytesForCurrentThread());

		// Assert to prevent code from being optimized out.
		Assert.Equal("A, B", ab);
		Assert.True("C, D"u8.SequenceEqual(cd));
		Assert.Equal("A, B", ab2);
		Assert.True("C, D"u8.SequenceEqual(cd2));
	}
}
