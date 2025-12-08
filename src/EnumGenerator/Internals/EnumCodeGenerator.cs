using EnumGenerator.Internals.Model;
using EnumGenerator.Internals.Utils;

namespace EnumGenerator.Internals;

internal sealed class EnumCodeGenerator(EnumModel enumModel)
{
	public string Generate()
	{
		if (enumModel.HasFlagsAttribute)
			throw new NotSupportedException("Flags enums are not supported. Use the FlagEnumCodeGenerator instead.");

		CodeWriter writer = new();
		writer.AddUsingIfNeeded(enumModel, "System");
		writer.AddUsingIfNeeded(enumModel, "System.Collections.Generic");
		writer.AddUsingIfNeeded(enumModel, "System.IO");
		writer.WriteLine();
		writer.WriteLine($"namespace {enumModel.NamespaceName};");
		writer.WriteLine();
		writer.WriteLine($"{enumModel.Accessibility} static class {enumModel.GetClassName()}");
		writer.StartBlock();

		writer.GenerateDefinedValuesHashSet(enumModel);
		writer.WriteLine();

		writer.GenerateValuesProperty(enumModel);
		writer.WriteLine();

		writer.GenerateNullTerminatedMemberNamesProperty(enumModel);
		writer.WriteLine();

		writer.WriteLine($"public static string ToStringFast(this {enumModel.EnumTypeName} value)");
		writer.StartBlock();
		writer.WriteLine("return value switch");
		writer.StartBlock();
		foreach (EnumMemberModel member in enumModel.UniqueMembers)
			writer.WriteLine($"{enumModel.EnumTypeName}.{member.Name} => \"{member.DisplayName}\",");
		writer.WriteLine("_ => throw new ArgumentOutOfRangeException(nameof(value), value, null),");
		writer.EndBlockWithSemicolon();
		writer.EndBlock();
		writer.WriteLine();

		writer.WriteLine($"public static ReadOnlySpan<byte> AsUtf8Span(this {enumModel.EnumTypeName} value)");
		writer.StartBlock();
		writer.WriteLine("return value switch");
		writer.StartBlock();
		foreach (EnumMemberModel member in enumModel.UniqueMembers)
			writer.WriteLine($"{enumModel.EnumTypeName}.{member.Name} => \"{member.DisplayName}\"u8,");
		writer.WriteLine("_ => throw new ArgumentOutOfRangeException(nameof(value), value, null),");
		writer.EndBlockWithSemicolon();
		writer.EndBlock();
		writer.WriteLine();

		writer.WriteLine($"public static {enumModel.EnumTypeName} FromStringFast(string value)");
		writer.StartBlock();
		writer.WriteLine("return value switch");
		writer.StartBlock();
		foreach (EnumMemberModel member in enumModel.UniqueMembers)
			writer.WriteLine($"\"{member.DisplayName}\" => {enumModel.EnumTypeName}.{member.Name},");
		writer.WriteLine("_ => throw new ArgumentOutOfRangeException(nameof(value), value, null),");
		writer.EndBlockWithSemicolon();
		writer.EndBlock();
		writer.WriteLine();

		writer.GenerateGetIndexMethod(enumModel);
		writer.WriteLine();

		writer.GenerateFromIndexMethod(enumModel);
		writer.WriteLine();

		writer.GenerateWriteMethod(enumModel);
		writer.WriteLine();

		writer.GenerateReadMethod(enumModel);
		writer.WriteLine();

		writer.GenerateIsDefinedMethod(enumModel);

		writer.EndBlock();

		return writer.ToString();
	}
}
