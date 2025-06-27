using EnumGenerator.Internals.Model;
using EnumGenerator.Internals.Utils;

namespace EnumGenerator.Internals;

internal static class CodeGeneratorUtils
{
	public static void AddUsingIfNeeded(this CodeWriter writer, EnumModel enumModel, string usingNamespace)
	{
		if (enumModel.NamespaceName == usingNamespace)
			return;

		writer.WriteLine($"using {usingNamespace};");
	}

	public static string GetClassName(this EnumModel enumModel)
	{
		// TODO: Maybe also check if GeneratedClassName is a valid C# identifier.
		if (string.IsNullOrWhiteSpace(enumModel.GeneratedClassName))
			return $"{enumModel.EnumName}Gen";

		// ! IsNullOrWhiteSpace is not annotated in .NET Standard 2.0.
		return enumModel.GeneratedClassName!;
	}

	public static void GenerateValuesProperty(this CodeWriter writer, EnumModel enumModel)
	{
		writer.WriteLine($$"""public static IReadOnlyList<{{enumModel.EnumTypeName}}> Values { get; } = Enum.GetValues<{{enumModel.EnumTypeName}}>();""");
	}

	public static void GenerateNullTerminatedMemberNamesProperty(this CodeWriter writer, List<EnumMemberModel> uniqueMembers)
	{
		if (uniqueMembers.Count == 0)
			return;

		string nullTerminatedMemberNames = string.Concat(uniqueMembers.Select(kvp => $"{kvp.DisplayName}\\0"));
		writer.WriteLine($"public static ReadOnlySpan<byte> NullTerminatedMemberNames => \"{nullTerminatedMemberNames}\"u8;");
		writer.WriteLine();
	}

	public static void GenerateGetIndexMethod(this CodeWriter writer, EnumModel enumModel, List<EnumMemberModel> uniqueMembers)
	{
		writer.WriteLine($"public static int GetIndex(this {enumModel.EnumTypeName} value)");
		writer.StartBlock();
		writer.WriteLine("return value switch");
		writer.StartBlock();
		for (int i = 0; i < uniqueMembers.Count; i++)
		{
			EnumMemberModel member = uniqueMembers[i];
			writer.WriteLine($"{enumModel.EnumTypeName}.{member.Name} => {i},");
		}

		writer.WriteLine("_ => throw new ArgumentOutOfRangeException(nameof(value), value, null),");
		writer.EndBlockWithSemicolon();
		writer.EndBlock();
	}

	public static void GenerateFromIndexMethod(this CodeWriter writer, EnumModel enumModel, List<EnumMemberModel> uniqueMembers)
	{
		writer.WriteLine($"public static {enumModel.EnumTypeName} FromIndex(int index)");
		writer.StartBlock();
		writer.WriteLine("return index switch");
		writer.StartBlock();
		for (int i = 0; i < uniqueMembers.Count; i++)
		{
			EnumMemberModel member = uniqueMembers[i];
			writer.WriteLine($"{i} => {enumModel.EnumTypeName}.{member.Name},");
		}

		writer.WriteLine("_ => throw new ArgumentOutOfRangeException(nameof(index), index, null),");
		writer.EndBlockWithSemicolon();
		writer.EndBlock();
	}

	public static void GenerateWriteMethod(this CodeWriter writer, EnumModel enumModel)
	{
		writer.WriteLine($"public static void Write(this BinaryWriter writer, {enumModel.EnumTypeName} value)");
		writer.StartBlock();
		writer.WriteLine($"writer.Write(({enumModel.EnumUnderlyingTypeName})value);");
		writer.EndBlock();
	}

	public static void GenerateReadMethod(this CodeWriter writer, EnumModel enumModel)
	{
		writer.WriteLine($"public static {enumModel.EnumTypeName} Read{enumModel.EnumName}(this BinaryReader reader)");
		writer.StartBlock();
		writer.WriteLine($"return ({enumModel.EnumTypeName})reader.{enumModel.BinaryReaderMethodName}();");
		writer.EndBlock();
	}
}
