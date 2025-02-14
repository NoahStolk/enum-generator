namespace EnumGenerator;

[AttributeUsage(AttributeTargets.Enum)]
public sealed class GenerateEnumUtilitiesAttribute : Attribute
{
	public GenerateEnumUtilitiesAttribute()
	{
	}

	public GenerateEnumUtilitiesAttribute(string? generatedClassName)
	{
		GeneratedClassName = generatedClassName;
	}

	public string? GeneratedClassName { get; set; }
}
