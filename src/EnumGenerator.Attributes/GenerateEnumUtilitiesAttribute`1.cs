namespace EnumGenerator;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public sealed class GenerateEnumUtilitiesAttribute<T> : Attribute
	where T : Enum
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
