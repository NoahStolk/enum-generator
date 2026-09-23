namespace EnumGenerator.Tests.Integration.Enums;

[Flags]
[GenerateEnumUtilities]
internal enum WideFlagsType : ushort
{
	F0 = 1 << 0,
	F1 = 1 << 1,
	F2 = 1 << 2,
	F3 = 1 << 3,
	F4 = 1 << 4,
	F5 = 1 << 5,
	F6 = 1 << 6,
	F7 = 1 << 7,
	F8 = 1 << 8,
	F9 = 1 << 9,
	F10 = 1 << 10,
	F11 = 1 << 11,
	F12 = 1 << 12,
	F13 = 1 << 13,
	F14 = 1 << 14,
	F15 = 1 << 15,
}
