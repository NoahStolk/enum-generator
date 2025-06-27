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

		Dictionary<BigInteger, string> flagValues = GetFlagValues(uniqueMembers);
		int bitCount = enumModel.EnumUnderlyingTypeName switch
		{
			"byte" or "sbyte" => 8,
			"short" or "ushort" => 16,
			"int" or "uint" => 32,
			"long" or "ulong" => 64,
			_ => throw new InvalidOperationException($"Unsupported enum underlying type: {enumModel.EnumUnderlyingTypeName}"),
		};
		bool isExhaustive = uniqueMembers.Count + flagValues.Count == (int)Math.Pow(2, bitCount);

		CodeWriter writer = new();
		writer.AddUsingIfNeeded(enumModel, "System");
		writer.AddUsingIfNeeded(enumModel, "System.Collections.Generic");
		writer.AddUsingIfNeeded(enumModel, "System.IO");
		writer.WriteLine();
		writer.WriteLine($"namespace {enumModel.NamespaceName};");
		writer.WriteLine();
		writer.WriteLine($"{enumModel.Accessibility} static class {enumModel.GetClassName()}");
		writer.StartBlock();

		writer.GenerateValuesProperty(enumModel);
		writer.WriteLine();

		writer.GenerateNullTerminatedMemberNamesProperty(uniqueMembers);

		writer.WriteLine($"public static string ToStringFast(this {enumModel.EnumTypeName} value)");
		writer.StartBlock();
		writer.WriteLine("return value switch");
		writer.StartBlock();
		foreach (EnumMemberModel member in uniqueMembers)
			writer.WriteLine($"{enumModel.EnumTypeName}.{member.Name} => \"{member.DisplayName}\",");
		foreach (KeyValuePair<BigInteger, string> flagKvp in flagValues)
			writer.WriteLine($"({enumModel.EnumTypeName}){flagKvp.Key} => \"{flagKvp.Value}\",");
		if (!isExhaustive)
			writer.WriteLine("_ => throw new ArgumentOutOfRangeException(nameof(value), value, null),");
		writer.EndBlockWithSemicolon();
		writer.EndBlock();
		writer.WriteLine();
		writer.WriteLine($"public static ReadOnlySpan<byte> AsUtf8Span(this {enumModel.EnumTypeName} value)");
		writer.StartBlock();
		writer.WriteLine("return value switch");
		writer.StartBlock();
		foreach (EnumMemberModel member in uniqueMembers)
			writer.WriteLine($"{enumModel.EnumTypeName}.{member.Name} => \"{member.DisplayName}\"u8,");
		foreach (KeyValuePair<BigInteger, string> flagKvp in flagValues)
			writer.WriteLine($"({enumModel.EnumTypeName}){flagKvp.Key} => \"{flagKvp.Value}\"u8,");
		if (!isExhaustive)
			writer.WriteLine("_ => throw new ArgumentOutOfRangeException(nameof(value), value, null),");
		writer.EndBlockWithSemicolon();
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

	private static Dictionary<BigInteger, string> GetFlagValues(List<EnumMemberModel> enumValues)
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

			combinations[value] = string.Join(", ", parts);
		}

		return combinations.Where(kvp => kvp.Key != 0 && !IsPowerOfTwo(kvp.Key)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
	}

	private static bool IsPowerOfTwo(BigInteger value)
	{
		return value != 0 && (value & (value - 1)) == 0;
	}
}
