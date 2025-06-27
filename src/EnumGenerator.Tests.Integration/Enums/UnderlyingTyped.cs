using System.Diagnostics.CodeAnalysis;

namespace EnumGenerator.Tests.Integration.Enums;

[SuppressMessage("Design", "CA1028:Enum Storage should be Int32", Justification = "Testing purposes")]
[GenerateEnumUtilities]
internal enum UnderlyingTypedByte : byte
{
	A,
	B,
	C,
}

[SuppressMessage("Design", "CA1028:Enum Storage should be Int32", Justification = "Testing purposes")]
[GenerateEnumUtilities]
internal enum UnderlyingTypedUShort : ushort
{
	A,
	B,
	C,
}

[SuppressMessage("Design", "CA1028:Enum Storage should be Int32", Justification = "Testing purposes")]
[GenerateEnumUtilities]
internal enum UnderlyingTypedUInt : uint
{
	A,
	B,
	C,
}

[SuppressMessage("Design", "CA1028:Enum Storage should be Int32", Justification = "Testing purposes")]
[GenerateEnumUtilities]
internal enum UnderlyingTypedULong : ulong
{
	A,
	B,
	C,
}

[SuppressMessage("Design", "CA1028:Enum Storage should be Int32", Justification = "Testing purposes")]
[GenerateEnumUtilities]
internal enum UnderlyingTypedSByte : sbyte
{
	A,
	B,
	C,
}

[SuppressMessage("Design", "CA1028:Enum Storage should be Int32", Justification = "Testing purposes")]
[GenerateEnumUtilities]
internal enum UnderlyingTypedShort : short
{
	A,
	B,
	C,
}

[GenerateEnumUtilities]
internal enum UnderlyingTypedInt
{
	A,
	B,
	C,
}

[SuppressMessage("Design", "CA1028:Enum Storage should be Int32", Justification = "Testing purposes")]
[GenerateEnumUtilities]
internal enum UnderlyingTypedLong : long
{
	A,
	B,
	C,
}
