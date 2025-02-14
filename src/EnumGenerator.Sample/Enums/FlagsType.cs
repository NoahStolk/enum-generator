namespace EnumGenerator.Sample.Enums;

[Flags]
[GenerateEnumUtilities]
internal enum FlagsType
{
	None = 0,
	A = 1,
	B = 2,
	C = 4,
	D = 8,
	E = 16,
}
