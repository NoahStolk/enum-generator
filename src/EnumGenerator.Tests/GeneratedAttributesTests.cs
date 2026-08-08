using EnumGenerator.Tests.Utils;

namespace EnumGenerator.Tests;

/// <summary>
/// The attributes are emitted into the consuming compilation rather than shipped as a reference assembly, which makes
/// them part of the generator's output. <see cref="TestHelper.Verify"/> filters them out of the per-enum snapshots to
/// avoid duplicating them across every test; this verifies them once.
/// </summary>
public sealed class GeneratedAttributesTests
{
	[Fact]
	public async Task GeneratedAttributes()
	{
		string code =
			"""
			using EnumGenerator;
			namespace Tests;
			[GenerateEnumUtilities]
			internal enum TestEnum
			{
				None,
			}
			""";

		await TestHelper.VerifyIncludingAttributes(code);
	}
}
