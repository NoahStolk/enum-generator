namespace EnumGenerator.Tests.Integration.Enums;

[Flags]
[GenerateEnumUtilities]
internal enum FlagsTypeWithMissingBits
{
	None = 0,
	A = 1,
	B = 2,
	D = 8,
	E = 16,
}
