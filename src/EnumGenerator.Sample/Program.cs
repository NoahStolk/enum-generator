using EnumGenerator.Sample.Enums;
using System.Text;

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

FlagsType ab = FlagsType.A | FlagsType.B;
Console.WriteLine(ab.HasFlagFast(FlagsType.A));
Console.WriteLine(ab.HasFlagFast(FlagsType.B));
Console.WriteLine(ab.HasFlagFast(FlagsType.C));
Console.WriteLine(ab.HasFlagFast(FlagsType.D));
Console.WriteLine(ab.HasFlagFast(FlagsType.E));
Console.WriteLine();

static string SpanToString(ReadOnlySpan<byte> utf8)
{
	return Encoding.UTF8.GetString(utf8);
}
