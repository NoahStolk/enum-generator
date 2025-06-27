using System.ComponentModel.DataAnnotations;

namespace EnumGenerator.Tests.Integration.Enums;

[Flags]
[GenerateEnumUtilities]
internal enum NamedFlagsType
{
	[Display(Name = "No value")]
	None = 0,

	[Display(Name = "Value A")]
	A = 1,

	[Display(Name = "Value B")]
	B = 2,

	[Display(Name = "Value C")]
	C = 4,

	[Display(Name = "Value D")]
	D = 8,

	[Display(Name = "Value E")]
	E = 16,
}
