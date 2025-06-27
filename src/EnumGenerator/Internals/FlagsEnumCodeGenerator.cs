using EnumGenerator.Internals.Extensions;
using EnumGenerator.Internals.Model;
using EnumGenerator.Internals.Utils;
using System.Numerics;

namespace EnumGenerator.Internals;

internal sealed class FlagsEnumCodeGenerator(EnumModel enumModel)
{
	public string Generate()
	{
		if (!enumModel.HasFlagsAttribute)
			throw new NotSupportedException("Only flags enums are supported. Use the EnumCodeGenerator instead.");

		List<EnumMemberModel> uniqueMembers = enumModel.Members
			.Where(member => member.ConstantValue == 0 || IsPowerOfTwo(member.ConstantValue))
			.DistinctBy(m => m.ConstantValue)
			.ToList();

		Dictionary<BigInteger, string> flagValues = GetCombinedStringValues(uniqueMembers);

		CodeWriter writer = new();
		writer.AddUsingIfNeeded(enumModel, "System");
		writer.AddUsingIfNeeded(enumModel, "System.Collections.Generic");
		writer.AddUsingIfNeeded(enumModel, "System.IO");
		writer.AddUsingIfNeeded(enumModel, "System.Text");
		writer.WriteLine();
		writer.WriteLine($"namespace {enumModel.NamespaceName};");
		writer.WriteLine();
		writer.WriteLine($"{enumModel.Accessibility} static class {enumModel.GetClassName()}");
		writer.StartBlock();

		writer.WriteLine($"private static readonly Dictionary<{enumModel.EnumTypeName}, string> _stringValues = new()");
		writer.StartBlock();
		foreach (KeyValuePair<BigInteger, string> flagKvp in flagValues)
			writer.WriteLine($"{{ ({enumModel.EnumTypeName}){flagKvp.Key}, \"{flagKvp.Value}\" }},");
		writer.EndBlockWithSemicolon();
		writer.WriteLine($"private static readonly Dictionary<{enumModel.EnumTypeName}, byte[]> _utf8Cache = new();");
		writer.WriteLine();

		writer.GenerateValuesProperty(enumModel);
		writer.WriteLine();

		writer.GenerateNullTerminatedMemberNamesProperty(uniqueMembers);

		writer.WriteLine($"public static string ToStringFast(this {enumModel.EnumTypeName} value)");
		writer.StartBlock();
		writer.WriteLine("return _stringValues.TryGetValue(value, out string? stringValue) ? stringValue : throw new ArgumentOutOfRangeException(nameof(value), value, null);");
		writer.EndBlock();
		writer.WriteLine();

		writer.WriteLine($"public static ReadOnlySpan<byte> AsUtf8Span(this {enumModel.EnumTypeName} value)");
		writer.StartBlock();
		writer.WriteLine("if (!_stringValues.TryGetValue(value, out string? str))");
		writer.StartIndent();
		writer.WriteLine("throw new ArgumentOutOfRangeException(nameof(value), value, null);");
		writer.EndIndent();
		writer.WriteLine();
		writer.WriteLine("if (!_utf8Cache.TryGetValue(value, out byte[]? bytes))");
		writer.StartBlock();
		writer.WriteLine("bytes = Encoding.UTF8.GetBytes(str);");
		writer.WriteLine("_utf8Cache[value] = bytes;");
		writer.EndBlock();
		writer.WriteLine();
		writer.WriteLine("return new ReadOnlySpan<byte>(bytes);");
		writer.EndBlock();
		writer.WriteLine();

		writer.GenerateGetIndexMethod(enumModel, uniqueMembers);
		writer.WriteLine();

		writer.GenerateFromIndexMethod(enumModel, uniqueMembers);
		writer.WriteLine();

		writer.GenerateWriteMethod(enumModel);
		writer.WriteLine();

		writer.GenerateReadMethod(enumModel);

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

	private static Dictionary<BigInteger, string> GetCombinedStringValues(List<EnumMemberModel> enumValues)
	{
		List<EnumMemberModel> flagValues = enumValues
			.Where(v => v.ConstantValue == 0 || IsPowerOfTwo(v.ConstantValue))
			.ToList();

		Dictionary<BigInteger, string> combinations = new();
		int count = flagValues.Count;
		for (int i = 0; i < 1 << count; i++)
		{
			BigInteger value = 0;
			List<string> parts = [];
			for (int j = 0; j < count; j++)
			{
				if (((i >> j) & 1) == 0)
					continue;

				BigInteger val = flagValues[j].ConstantValue;
				if (val == 0)
					continue;

				value |= val;
				parts.Add(enumValues.Find(v => v.ConstantValue == val).DisplayName);
			}

			if (value == 0)
				combinations[value] = enumValues.Find(v => v.ConstantValue == 0).DisplayName;
			else
				combinations[value] = string.Join(", ", parts);
		}

		return combinations;
	}

	private static bool IsPowerOfTwo(BigInteger value)
	{
		return value != 0 && (value & (value - 1)) == 0;
	}
}
