using EnumGenerator.Internals.Extensions;
using EnumGenerator.Internals.Model;
using EnumGenerator.Internals.Utils;
using System.Numerics;

namespace EnumGenerator.Internals;

internal sealed class FlagsEnumCodeGenerator(EnumModel enumModel)
{
	private static bool IsPowerOfTwo(BigInteger value)
	{
		return value != 0 && (value & (value - 1)) == 0;
	}

	public string Generate()
	{
		if (!enumModel.HasFlagsAttribute)
			throw new NotSupportedException("Only flags enums are supported. Use the EnumCodeGenerator instead.");

		List<EnumMemberModel> uniqueMembers = enumModel.Members
			.Where(member => member.ConstantValue == 0 || IsPowerOfTwo(member.ConstantValue))
			.DistinctBy(m => m.ConstantValue)
			.ToList();

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

		writer.WriteLine($"private static readonly {enumModel.EnumTypeName} _definedBits =");
		writer.StartIndent();
		for (int i = 0; i < uniqueMembers.Count; i++)
		{
			EnumMemberModel member = uniqueMembers[i];
			if (i == uniqueMembers.Count - 1)
				writer.WriteLine($"{enumModel.EnumTypeName}.{member.Name};");
			else
				writer.WriteLine($"{enumModel.EnumTypeName}.{member.Name} |");
		}

		writer.WriteLine();
		writer.EndIndent();
		writer.WriteLine($"private static readonly Dictionary<{enumModel.EnumTypeName}, string> _stringCache = new()");
		writer.StartBlock();
		foreach (EnumMemberModel member in uniqueMembers)
			writer.WriteLine($"{{ {enumModel.EnumTypeName}.{member.Name}, \"{member.DisplayName}\" }},");
		writer.EndBlockWithSemicolon();
		writer.WriteLine();

		writer.WriteLine($"private static readonly Dictionary<{enumModel.EnumTypeName}, byte[]> _utf8Cache = new();");
		writer.WriteLine();

		writer.GenerateValuesProperty(enumModel);
		writer.WriteLine();

		writer.GenerateNullTerminatedMemberNamesProperty(uniqueMembers);

		writer.WriteLine($"public static string ToStringFast(this {enumModel.EnumTypeName} value)");
		writer.StartBlock();
		writer.WriteLine("if (_stringCache.TryGetValue(value, out string? str))");
		writer.StartIndent();
		writer.WriteLine("return str;");
		writer.EndIndent();
		writer.WriteLine();
		writer.WriteLine("str = GetFlagsString(value);");
		writer.WriteLine("_stringCache[value] = str;");
		writer.WriteLine("return str;");
		writer.EndBlock();
		writer.WriteLine();

		writer.WriteLine($"private static string GetFlagsString({enumModel.EnumTypeName} value)");
		writer.StartBlock();
		writer.WriteLine($"{enumModel.EnumUnderlyingTypeName} raw = ({enumModel.EnumUnderlyingTypeName})value;");
		writer.WriteLine("if (raw == 0)");
		writer.StartIndent();
		writer.WriteLine("throw new ArgumentOutOfRangeException(nameof(value), value, null); // This means 0 is not a defined member, otherwise it would have been cached");
		writer.EndIndent();
		writer.WriteLine();
		writer.WriteLine($"if ((raw & ~({enumModel.EnumUnderlyingTypeName})_definedBits) != 0)");
		writer.StartIndent();
		writer.WriteLine("throw new ArgumentOutOfRangeException(nameof(value), value, null);");
		writer.EndIndent();
		writer.WriteLine();
		writer.WriteLine("List<string> names = new(Values.Count);");
		writer.WriteLine($"foreach ({enumModel.EnumTypeName} item in Values)");
		writer.StartBlock();
		writer.WriteLine($"{enumModel.EnumUnderlyingTypeName} itemRaw = ({enumModel.EnumUnderlyingTypeName})item;");
		writer.WriteLine("if (itemRaw != 0 && (raw & itemRaw) == itemRaw)");
		writer.StartIndent();
		writer.WriteLine("names.Add(_stringCache[item]); // Must be present in cache (defined flags are always pre-initialized)");
		writer.EndIndent();
		writer.EndBlock();
		writer.WriteLine();
		writer.WriteLine("return string.Join(\", \", names);");
		writer.EndBlock();
		writer.WriteLine();

		writer.WriteLine($"public static ReadOnlySpan<byte> AsUtf8Span(this {enumModel.EnumTypeName} value)");
		writer.StartBlock();
		writer.WriteLine("if (_utf8Cache.TryGetValue(value, out byte[]? bytes))");
		writer.StartIndent();
		writer.WriteLine("return new ReadOnlySpan<byte>(bytes);");
		writer.EndIndent();
		writer.WriteLine();
		writer.WriteLine("bytes = Encoding.UTF8.GetBytes(value.ToStringFast());");
		writer.WriteLine("_utf8Cache[value] = bytes;");
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
}
