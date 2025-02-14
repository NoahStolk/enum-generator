namespace EnumGenerator;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public sealed class GenerateEnumUtilitiesAttribute<T> : Attribute
	where T : Enum;
