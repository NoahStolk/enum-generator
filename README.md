# enum-generator

**🚧 WORK IN PROGRESS 🚧**

[![NuGet Version](https://img.shields.io/nuget/v/NoahStolk.EnumGenerator.svg)](https://www.nuget.org/packages/NoahStolk.EnumGenerator/)

Enum-generator is a zero-dependency library containing a source generator that generates useful extension methods and utilities for enums in C#.

## Features

- **Fast string conversions** — `ToStringFast()` compiles to a `switch` over the enum members instead of the reflection-based `Enum.ToString()`, and `FromStringFast(string)` does the reverse.
- **UTF-8 support** — `AsUtf8Span()` returns a `ReadOnlySpan<byte>` backed by a `u8` literal, so no allocation or encoding is needed. `NullTerminatedMemberNames` exposes all member names as a single null-terminated UTF-8 blob (useful for native interop, such as Dear ImGui combo boxes).
- **Allocation-free `Values`** — `Values` is a cached `IReadOnlyList<TEnum>`, unlike `Enum.GetValues<TEnum>()` which allocates a new array on every call.
- **Fast validation** — `IsDefined()` checks a `HashSet` of the underlying values instead of using reflection.
- **Flags support** — flags enums get `HasFlagFast()` (no boxing, unlike `Enum.HasFlag`) and `ContainsDefinedFlagsOnly()`, and their `ToStringFast()` composes and caches combined names like `"A, B"`.
- **Index mapping** — `GetIndex()` and `FromIndex(int)` map members to their declaration order, which is handy for arrays and UI lists.
- **Binary serialization** — `BinaryWriter.Write(TEnum)` and `BinaryReader.Read{EnumName}()` round-trip the enum using its underlying type, so a `byte` enum takes one byte.
- **Custom display names** — `[Display(Name = "...")]` on a member overrides the name used by all string conversions.
- **Enums you don't own** — assembly-level `[GenerateEnumUtilities<T>]` generates utilities for enums from the BCL or third-party libraries.
- **All underlying types** — `byte`, `sbyte`, `short`, `ushort`, `int`, `uint`, `long`, and `ulong` are supported.
- **Zero dependencies** — the package contains nothing but the source generator. The attributes are generated into your own project, so there is no assembly to reference and nothing is added to your runtime dependencies.

## Examples

### Installation

```sh
dotnet add package NoahStolk.EnumGenerator
```

This is an analyzer-only package, so the `PackageReference` it writes needs no further editing:

```xml
<PackageReference Include="NoahStolk.EnumGenerator">
  <PrivateAssets>all</PrivateAssets>
  <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
</PackageReference>
```

The generated code uses UTF-8 string literals and `Enum.GetValues<T>()`, so consuming projects need C# 11 or newer and .NET 5 or newer. The analyzer itself requires .NET SDK 9.0.3xx (or VS 2022 17.14) or newer.

The `[GenerateEnumUtilities]` attributes are generated into your compilation as `internal` types in the `EnumGenerator` namespace — there is no assembly to reference, and the package leaves no trace in your build output or in the dependencies of any package you publish.

### Getting started

Mark an enum with `[GenerateEnumUtilities]`:

```cs
using EnumGenerator;

namespace MyApp;

[GenerateEnumUtilities]
internal enum Language
{
	CSharp,
	CPlusPlus,
}
```

This generates a static `LanguageGen` class in the same namespace, with the same accessibility as the enum:

```cs
Language.CSharp.ToStringFast();      // "CSharp"
Language.CSharp.AsUtf8Span();        // "CSharp"u8
Language.CSharp.GetIndex();          // 0
Language.CSharp.IsDefined();         // true
((Language)7).IsDefined();           // false

LanguageGen.Values;                  // [Language.CSharp, Language.CPlusPlus]
LanguageGen.FromStringFast("C++");   // Language.CPlusPlus
LanguageGen.FromIndex(1);            // Language.CPlusPlus
LanguageGen.NullTerminatedMemberNames; // "CSharp\0CPlusPlus\0"u8
```

Methods that receive a value outside the enum's defined members throw an `ArgumentOutOfRangeException`.

### Custom display names

`[Display(Name = "...")]` overrides the name used by `ToStringFast`, `FromStringFast`, `AsUtf8Span`, and `NullTerminatedMemberNames`:

```cs
using System.ComponentModel.DataAnnotations;

[GenerateEnumUtilities]
internal enum Language
{
	[Display(Name = "C#")]
	CSharp,

	[Display(Name = "C++")]
	CPlusPlus,
}
```

```cs
Language.CSharp.ToStringFast();    // "C#"
LanguageGen.FromStringFast("C++"); // Language.CPlusPlus
```

### Flags enums

Enums marked with `[Flags]` get a different set of utilities — `HasFlagFast` and `ContainsDefinedFlagsOnly` instead of `FromStringFast` and `IsDefined`:

```cs
[Flags]
[GenerateEnumUtilities]
internal enum FlagsType
{
	None = 0,
	A = 1,
	B = 2,
	C = 4,
	D = 8,
	E = 16,
}
```

```cs
const FlagsType ab = FlagsType.A | FlagsType.B;

ab.HasFlagFast(FlagsType.A);       // true
ab.HasFlagFast(FlagsType.C);       // false
ab.ToStringFast();                 // "A, B"
ab.AsUtf8Span();                   // "A, B"u8
ab.ContainsDefinedFlagsOnly();     // true
((FlagsType)32).ContainsDefinedFlagsOnly(); // false
```

Composed names are built once and cached, so repeated `ToStringFast` calls on the same combination don't reallocate. Members that are not a power of two (aliases such as `All = A | B | C`) are skipped, and `0` is only included when a member is declared for it.

### Enums you don't own

Use the generic attribute at the assembly level to generate utilities for enums from other assemblies, such as the BCL or a NuGet package:

```cs
using EnumGenerator;
using Silk.NET.OpenGL;

[assembly: GenerateEnumUtilities<DayOfWeek>]
[assembly: GenerateEnumUtilities<BlendEquationModeEXT>]
```

```cs
DayOfWeek.Sunday.ToStringFast();          // "Sunday"
BlendEquationModeEXT.FuncAdd.GetIndex();  // 0
DayOfWeekGen.Values;                      // all seven days
```

The generated class is placed in the enum's own namespace (`System` for `DayOfWeek`) and is always `public`. Note that `[Display]` attributes are not read for external enums, since the generator only sees their metadata.

Enums with duplicate values (common in generated interop bindings, where several names map to the same constant) are deduplicated — the first declared member wins.

### Custom generated class name

The default class name is `{EnumName}Gen`. Both attributes accept an override, either positionally or as a named argument:

```cs
[GenerateEnumUtilities(GeneratedClassName = "LanguageUtils")]
internal enum Language { /* ... */ }

[assembly: GenerateEnumUtilities<DayOfWeek>(GeneratedClassName = "DayOfWeekUtils")]
```

### Generated output

For the `Language` enum from the first example, the generator emits `Language.g.cs`:

```cs
// <auto-generated>
// This code was generated by EnumGenerator.
// </auto-generated>

#nullable enable

using System;
using System.Collections.Generic;
using System.IO;

namespace MyApp;

internal static class LanguageGen
{
	private static readonly HashSet<int> _definedValues = new()
	{
		(int)MyApp.Language.CSharp,
		(int)MyApp.Language.CPlusPlus,
	};

	public static IReadOnlyList<MyApp.Language> Values { get; } = Enum.GetValues<MyApp.Language>();

	public static ReadOnlySpan<byte> NullTerminatedMemberNames => "CSharp\0CPlusPlus\0"u8;

	public static string ToStringFast(this MyApp.Language value)
	{
		return value switch
		{
			MyApp.Language.CSharp => "CSharp",
			MyApp.Language.CPlusPlus => "CPlusPlus",
			_ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
		};
	}

	// AsUtf8Span, FromStringFast, GetIndex, FromIndex, Write, ReadLanguage, IsDefined
}
```

## Benchmarks

TODO

## Development

### Debugging the Source Generator

To debug the source generator, use the `launchSettings.json` file in the `EnumGenerator` project to run the generator against the `EnumGenerator.Sample` project.

You can also debug the generator tests using the `EnumGenerator.Tests` project.

### Snapshot Testing

> To simply accept all snapshots immediately, run `./scripts/accept-all.sh src/EnumGenerator.Tests/snapshots` from the root of the repository.

To control which diff tool is used for snapshot testing, use the `DiffEngine_ToolOrder` environment variable.

In JetBrains Rider, this can be configured under Build, Execution, Deployment > Unit Testing > Test Runner > Environment variables.

You can also disable DiffEngine by setting the `DiffEngine_Disable` environment variable to `true`.

- [Verify Support plugin for Rider](https://plugins.jetbrains.com/plugin/17240-verify-support)
