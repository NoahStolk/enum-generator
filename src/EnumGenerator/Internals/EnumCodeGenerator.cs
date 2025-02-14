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
		// TODO: Maybe also check if the class name is a valid C# identifier.
		if (string.IsNullOrWhiteSpace(enumModel.GeneratedClassName))
			return $"{enumModel.EnumName}Utils";

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
		writer.WriteLine();
		writer.WriteLine($"namespace {enumModel.NamespaceName};");
		writer.WriteLine();
		writer.WriteLine($"{enumModel.Accessibility} static class {GetClassName()}");
		writer.StartBlock();

		writer.WriteLine($$"""public static IReadOnlyList<{{enumModel.EnumName}}> Values { get; } = Enum.GetValues<{{enumModel.EnumName}}>();""");
		writer.WriteLine();

		if (relevantMembers.Count > 0)
		{
			string nullTerminatedMemberNames = string.Concat(relevantMembers.Select(kvp => $"{kvp.DisplayName}\\0"));
			writer.WriteLine($"public static ReadOnlySpan<byte> NullTerminatedMemberNames => \"{nullTerminatedMemberNames}\"u8;");
			writer.WriteLine();
		}

		writer.WriteLine($"public static string ToStringFast(this {enumModel.EnumName} value)");
		writer.StartBlock();
		writer.WriteLine("return value switch");
		writer.StartBlock();
		foreach (EnumMemberModel member in relevantMembers)
			writer.WriteLine($"{enumModel.EnumName}.{member.Name} => \"{member.DisplayName}\",");
		writer.WriteLine("_ => throw new ArgumentOutOfRangeException(nameof(value), value, null),");
		writer.EndBlockWithSemicolon();
		writer.EndBlock();
		writer.WriteLine();
		writer.WriteLine($"public static ReadOnlySpan<byte> AsUtf8Span(this {enumModel.EnumName} value)");
		writer.StartBlock();
		writer.WriteLine("return value switch");
		writer.StartBlock();
		foreach (EnumMemberModel member in relevantMembers)
			writer.WriteLine($"{enumModel.EnumName}.{member.Name} => \"{member.DisplayName}\"u8,");
		writer.WriteLine("_ => throw new ArgumentOutOfRangeException(nameof(value), value, null),");
		writer.EndBlockWithSemicolon();
		writer.EndBlock();

		if (enumModel.HasFlagsAttribute)
		{
			writer.WriteLine();
			writer.WriteLine($"public static bool HasFlagFast(this {enumModel.EnumName} value, {enumModel.EnumName} flag)");
			writer.StartBlock();
			writer.WriteLine("return (value & flag) != 0;");
			writer.EndBlock();
		}

		writer.EndBlock();

		return writer.ToString();
	}
}
