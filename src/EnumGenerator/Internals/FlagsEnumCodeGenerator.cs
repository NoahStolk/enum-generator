using EnumGenerator.Internals.Extensions;
using EnumGenerator.Internals.Model;
using EnumGenerator.Internals.Utils;

namespace EnumGenerator.Internals;

internal sealed class FlagsEnumCodeGenerator(EnumModel enumModel)
{
	public string Generate()
	{
		if (!enumModel.HasFlagsAttribute)
			throw new NotSupportedException("Only flags enums are supported. Use the EnumCodeGenerator instead.");

		CodeWriter writer = new();
		writer.AddUsingIfNeeded(enumModel, "System");
		writer.AddUsingIfNeeded(enumModel, "System.Collections.Concurrent");
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
		for (int i = 0; i < enumModel.UniqueMembers.Count; i++)
		{
			EnumMemberModel member = enumModel.UniqueMembers[i];
			writer.WriteLine(
				i == enumModel.UniqueMembers.Count - 1
					? $"{enumModel.EnumTypeName}.{member.Name};"
					: $"{enumModel.EnumTypeName}.{member.Name} |");
		}

		writer.WriteLine();
		writer.EndIndent();
		writer.WriteLine($"private static readonly ConcurrentDictionary<{enumModel.EnumTypeName}, string> _stringCache = new()");
		writer.StartBlock();
		foreach (EnumMemberModel member in enumModel.UniqueMembers)
			writer.WriteLine($"[{enumModel.EnumTypeName}.{member.Name}] = \"{member.DisplayName}\",");
		writer.EndBlockWithSemicolon();
		writer.WriteLine();

		writer.WriteLine($"private static readonly ConcurrentDictionary<{enumModel.EnumTypeName}, byte[]> _utf8Cache = new();");
		writer.WriteLine();

		writer.GenerateValuesProperty(enumModel);
		writer.WriteLine();

		writer.GenerateNullTerminatedMemberNamesProperty(enumModel);
		writer.WriteLine();

		writer.WriteLine($"public static string ToStringFast(this {enumModel.EnumTypeName} value)");
		writer.StartBlock();
		writer.WriteLine("return _stringCache.GetOrAdd(value, GetFlagsString);");
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
		writer.WriteLine("byte[] bytes = _utf8Cache.GetOrAdd(value, GetNullTerminatedUtf8Bytes);");
		writer.WriteLine("return new ReadOnlySpan<byte>(bytes, 0, bytes.Length - 1);");
		writer.EndBlock();
		writer.WriteLine();

		writer.WriteLine($"private static byte[] GetNullTerminatedUtf8Bytes({enumModel.EnumTypeName} value)");
		writer.StartBlock();
		writer.WriteLine("// Allocate one extra byte so the buffer is null-terminated beyond the span, matching u8 literals.");
		writer.WriteLine("string str = value.ToStringFast();");
		writer.WriteLine("byte[] bytes = new byte[Encoding.UTF8.GetByteCount(str) + 1];");
		writer.WriteLine("Encoding.UTF8.GetBytes(str, bytes);");
		writer.WriteLine("return bytes;");
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

		writer.WriteLine($"public static bool HasFlagFast(this {enumModel.EnumTypeName} value, {enumModel.EnumTypeName} flag)");
		writer.StartBlock();
		writer.WriteLine("return (value & flag) != 0;");
		writer.EndBlock();

		writer.WriteLine();
		writer.WriteLine($"public static bool ContainsDefinedFlagsOnly(this {enumModel.EnumTypeName} value)");
		writer.StartBlock();
		writer.WriteLine($"{enumModel.EnumUnderlyingTypeName} raw = ({enumModel.EnumUnderlyingTypeName})value;");
		writer.WriteLine($"return (raw & ~({enumModel.EnumUnderlyingTypeName})_definedBits) == 0;");
		writer.EndBlock();

		writer.EndBlock();

		return writer.ToString();
	}
}
