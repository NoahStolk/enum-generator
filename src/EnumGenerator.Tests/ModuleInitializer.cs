using System.Runtime.CompilerServices;

namespace EnumGenerator.Tests;

internal static class ModuleInitializer
{
	[ModuleInitializer]
	public static void Init()
	{
		VerifySourceGenerators.Initialize();
	}
}
