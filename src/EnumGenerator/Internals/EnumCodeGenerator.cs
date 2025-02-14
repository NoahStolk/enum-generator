using EnumGenerator.Internals.Model;
using EnumGenerator.Internals.Utils;

namespace EnumGenerator.Internals;

internal sealed class EnumCodeGenerator(EnumModel enumModel)
{
	public string Generate()
	{
		CodeWriter writer = new();
		writer.WriteLine("using System;");
		writer.WriteLine("using System.Collections.Generic;");
		writer.WriteLine();
		writer.WriteLine($"namespace {enumModel.NamespaceName};");
		writer.WriteLine();
		writer.WriteLine($"{enumModel.Accessibility} static partial class {enumModel.EnumName}Extensions");
		writer.StartBlock();
		writer.WriteLine($"public static string ToStringFast(this {enumModel.EnumName} value)");
		writer.StartBlock();
		writer.WriteLine("return value switch");
		writer.StartBlock();
		foreach (KeyValuePair<string, string> kvp in enumModel.Members)
			writer.WriteLine($"{enumModel.EnumName}.{kvp.Key} => \"{kvp.Value}\",");
		writer.WriteLine("_ => throw new ArgumentOutOfRangeException(nameof(value), value, null),");
		writer.EndBlockWithSemicolon();
		writer.EndBlock();
		writer.WriteLine();
		writer.WriteLine($"public static ReadOnlySpan<byte> AsUtf8Span(this {enumModel.EnumName} value)");
		writer.StartBlock();
		writer.WriteLine("return value switch");
		writer.StartBlock();
		foreach (KeyValuePair<string, string> kvp in enumModel.Members)
			writer.WriteLine($"{enumModel.EnumName}.{kvp.Key} => \"{kvp.Value}\"u8,");
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

		writer.WriteLine();
		writer.WriteLine($"{enumModel.Accessibility} static partial class {enumModel.EnumName}Utils");
		writer.StartBlock();
		writer.WriteLine($$"""public static IReadOnlyList<{{enumModel.EnumName}}> Values { get; } = Enum.GetValues<{{enumModel.EnumName}}>();""");
		writer.WriteLine();
		string nullTerminatedMemberNames = string.Concat(enumModel.Members.Select(kvp => $"{kvp.Value}\\0"));
		writer.WriteLine($"public static ReadOnlySpan<byte> NullTerminatedMemberNames => \"{nullTerminatedMemberNames}\"u8;");
		writer.EndBlock();

		return writer.ToString();
	}
}
