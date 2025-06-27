using EnumGenerator.Internals.Model;
using EnumGenerator.Internals.Utils;

namespace EnumGenerator.Internals;

internal sealed class EnumCodeGenerator(EnumModel enumModel)
{
	private void AddUsingIfNeeded(CodeWriter writer, string usingNamespace)
	{
		if (enumModel.NamespaceName == usingNamespace)
			return;

		writer.WriteLine($"using {usingNamespace};");
	}

	private string GetClassName()
	{
		// TODO: Maybe also check if GeneratedClassName is a valid C# identifier.
		if (string.IsNullOrWhiteSpace(enumModel.GeneratedClassName))
			return $"{enumModel.EnumName}Gen";

		// ! IsNullOrWhiteSpace is not annotated in .NET Standard 2.0.
		return enumModel.GeneratedClassName!;
	}

	public string Generate()
	{
		List<EnumMemberModel> relevantMembers = [];
		foreach (EnumMemberModel member in enumModel.Members)
		{
			if (member.ConstantValue != null && relevantMembers.Any(m => m.ConstantValue == member.ConstantValue))
				continue;

			relevantMembers.Add(member);
		}

		CodeWriter writer = new();
		AddUsingIfNeeded(writer, "System");
		AddUsingIfNeeded(writer, "System.Collections.Generic");
		AddUsingIfNeeded(writer, "System.IO");
		writer.WriteLine();
		writer.WriteLine($"namespace {enumModel.NamespaceName};");
		writer.WriteLine();
		writer.WriteLine($"{enumModel.Accessibility} static class {GetClassName()}");
		writer.StartBlock();

		writer.WriteLine($$"""public static IReadOnlyList<{{enumModel.EnumTypeName}}> Values { get; } = Enum.GetValues<{{enumModel.EnumTypeName}}>();""");
		writer.WriteLine();

		if (relevantMembers.Count > 0)
		{
			string nullTerminatedMemberNames = string.Concat(relevantMembers.Select(kvp => $"{kvp.DisplayName}\\0"));
			writer.WriteLine($"public static ReadOnlySpan<byte> NullTerminatedMemberNames => \"{nullTerminatedMemberNames}\"u8;");
			writer.WriteLine();
		}

		writer.WriteLine($"public static string ToStringFast(this {enumModel.EnumTypeName} value)");
		writer.StartBlock();
		writer.WriteLine("return value switch");
		writer.StartBlock();
		foreach (EnumMemberModel member in relevantMembers)
			writer.WriteLine($"{enumModel.EnumTypeName}.{member.Name} => \"{member.DisplayName}\",");
		writer.WriteLine("_ => throw new ArgumentOutOfRangeException(nameof(value), value, null),");
		writer.EndBlockWithSemicolon();
		writer.EndBlock();
		writer.WriteLine();
		writer.WriteLine($"public static ReadOnlySpan<byte> AsUtf8Span(this {enumModel.EnumTypeName} value)");
		writer.StartBlock();
		writer.WriteLine("return value switch");
		writer.StartBlock();
		foreach (EnumMemberModel member in relevantMembers)
			writer.WriteLine($"{enumModel.EnumTypeName}.{member.Name} => \"{member.DisplayName}\"u8,");
		writer.WriteLine("_ => throw new ArgumentOutOfRangeException(nameof(value), value, null),");
		writer.EndBlockWithSemicolon();
		writer.EndBlock();

		writer.WriteLine();
		writer.WriteLine($"public static int GetIndex(this {enumModel.EnumTypeName} value)");
		writer.StartBlock();
		writer.WriteLine("return value switch");
		writer.StartBlock();
		for (int i = 0; i < relevantMembers.Count; i++)
		{
			EnumMemberModel member = relevantMembers[i];
			writer.WriteLine($"{enumModel.EnumTypeName}.{member.Name} => {i},");
		}

		writer.WriteLine("_ => throw new ArgumentOutOfRangeException(nameof(value), value, null),");
		writer.EndBlockWithSemicolon();
		writer.EndBlock();

		writer.WriteLine();
		writer.WriteLine($"public static {enumModel.EnumTypeName} FromIndex(int index)");
		writer.StartBlock();
		writer.WriteLine("return index switch");
		writer.StartBlock();
		for (int i = 0; i < relevantMembers.Count; i++)
		{
			EnumMemberModel member = relevantMembers[i];
			writer.WriteLine($"{i} => {enumModel.EnumTypeName}.{member.Name},");
		}

		writer.WriteLine("_ => throw new ArgumentOutOfRangeException(nameof(index), index, null),");
		writer.EndBlockWithSemicolon();
		writer.EndBlock();

		writer.WriteLine();
		writer.WriteLine($"public static void Write{enumModel.EnumName}(this BinaryWriter writer, {enumModel.EnumTypeName} value)");
		writer.StartBlock();
		writer.WriteLine($"writer.Write(({enumModel.EnumUnderlyingTypeName})value);");
		writer.EndBlock();

		writer.WriteLine();
		writer.WriteLine($"public static {enumModel.EnumTypeName} Read{enumModel.EnumName}(this BinaryReader reader)");
		writer.StartBlock();
		writer.WriteLine($"return ({enumModel.EnumTypeName})reader.{enumModel.BinaryReaderMethodName}();");
		writer.EndBlock();

		if (enumModel.HasFlagsAttribute)
		{
			writer.WriteLine();
			writer.WriteLine($"public static bool HasFlagFast(this {enumModel.EnumTypeName} value, {enumModel.EnumTypeName} flag)");
			writer.StartBlock();
			writer.WriteLine("return (value & flag) != 0;");
			writer.EndBlock();
		}

		writer.EndBlock();

		return writer.ToString();
	}
}
