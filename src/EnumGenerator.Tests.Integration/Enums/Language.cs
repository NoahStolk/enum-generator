using System.ComponentModel.DataAnnotations;

namespace EnumGenerator.Tests.Integration.Enums;

[GenerateEnumUtilities]
internal enum Language
{
	[Display(Name = "C#")]
	CSharp,

	[Display(Name = "C++")]
	CPlusPlus,
}
