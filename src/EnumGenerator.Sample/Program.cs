using EnumGenerator;
using EnumGenerator.Sample.Enums;
using Silk.NET.OpenGL;
using System.Text;

[assembly: GenerateEnumUtilities<DayOfWeek>]
[assembly: GenerateEnumUtilities<BlendEquationModeEXT>]

Console.WriteLine(IntegerType.Byte.ToStringFast());
Console.WriteLine(IntegerType.Short.ToStringFast());
Console.WriteLine(IntegerType.Int.ToStringFast());
Console.WriteLine(IntegerType.Long.ToStringFast());
Console.WriteLine();

Console.WriteLine(SpanToString(IntegerType.Byte.AsUtf8Span()));
Console.WriteLine(SpanToString(IntegerType.Short.AsUtf8Span()));
Console.WriteLine(SpanToString(IntegerType.Int.AsUtf8Span()));
Console.WriteLine(SpanToString(IntegerType.Long.AsUtf8Span()));
Console.WriteLine();

const FlagsType ab = FlagsType.A | FlagsType.B;
Console.WriteLine(ab.HasFlagFast(FlagsType.A));
Console.WriteLine(ab.HasFlagFast(FlagsType.B));
Console.WriteLine(ab.HasFlagFast(FlagsType.C));
Console.WriteLine(ab.HasFlagFast(FlagsType.D));
Console.WriteLine(ab.HasFlagFast(FlagsType.E));
Console.WriteLine();

Console.WriteLine(Language.CSharp.ToStringFast());
Console.WriteLine(Language.CPlusPlus.ToStringFast());
Console.WriteLine();

Console.WriteLine(DayOfWeek.Sunday.ToStringFast());
Console.WriteLine();

Console.WriteLine(BlendEquationModeEXT.FuncAdd.ToStringFast());

static string SpanToString(ReadOnlySpan<byte> utf8)
{
	return Encoding.UTF8.GetString(utf8);
}
