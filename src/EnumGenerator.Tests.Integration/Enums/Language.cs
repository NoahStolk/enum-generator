using System.ComponentModel.DataAnnotations;

namespace EnumGenerator.Sample.Enums;

[GenerateEnumUtilities]
internal enum Language
{
	[Display(Name = "C#")]
	CSharp,

	[Display(Name = "C++")]
	CPlusPlus,
}
