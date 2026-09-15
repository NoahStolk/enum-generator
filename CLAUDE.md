# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build & test

The solution file is `src/EnumGenerator.slnx` (the newer slnx format) and the SDK is pinned via `global.json` (10.0.100, `rollForward: latestMinor`). All commands below are run from the repo root unless noted.

- Build: `dotnet build src/EnumGenerator.slnx -c Release`
- Run all tests: `dotnet test --solution src/EnumGenerator.slnx -c Release --no-build`
- Run a single test class/method: `dotnet test --project src/EnumGenerator.Tests/EnumGenerator.Tests.csproj -c Release --no-build -- --treenode-filter "/*/*/EnumIncrementalGeneratorTests/EnumWithFlagsAttribute"`

  Tests run on TUnit, which uses Microsoft.Testing.Platform rather than VSTest. Two consequences bite if you
  forget them: `dotnet test` needs `--solution`/`--project` instead of a bare path (a positional path is
  rejected), and VSTest's `--filter "FullyQualifiedName~X"` is **silently ignored** — MTP prints its help and
  exits reporting zero tests, which reads like a pass. Use
  `--treenode-filter "/<Assembly>/<Namespace>/<Class>/<Test>"`. `test.runner` in `global.json` is what puts
  `dotnet test` into MTP mode; without it nothing runs at all.
- Run the sample (useful for eyeballing generated output): `dotnet run --project src/EnumGenerator.Sample`

### NuGet integration tests

`EnumGenerator.Tests.NuGetIntegration` is intentionally **not** in the slnx — it consumes the generator as a packed NuGet rather than as a project reference. To run it the way CI does (see `.github/workflows/push.yml`):

```bash
cd src/
dotnet pack -c Release -o ./artifacts -p:Version=0.0.0-temp
dotnet restore EnumGenerator.Tests.NuGetIntegration/EnumGenerator.Tests.NuGetIntegration.csproj --packages ./packages --configfile "nuget.integration-tests.config"
dotnet build EnumGenerator.Tests.NuGetIntegration/EnumGenerator.Tests.NuGetIntegration.csproj -c Release --packages ./packages --no-restore
dotnet test  --project EnumGenerator.Tests.NuGetIntegration/EnumGenerator.Tests.NuGetIntegration.csproj -c Release --no-build --no-restore
```

The project links the integration-test `.cs` files from `EnumGenerator.Tests.Integration` via wildcard `<Compile Include="..\EnumGenerator.Tests.Integration\**\*.cs" Link="..." />` — keep the two test surfaces parallel.

### Snapshot tests

`EnumGenerator.Tests` uses Verify.SourceGenerators + Verify.TUnit. Snapshots live in `src/EnumGenerator.Tests/snapshots/` (relocated via `UseDirectory(Path.Combine("..", "snapshots"))` in `TestHelper`).

Parameterized snapshot names (`EnumIncrementalGeneratorTests.Enum_accessibility=internal,byte#TestEnum.g.verified.cs`)
come from `TestHelper.Verify(code, params string[] args)` calling `UseParameters(args)` — the values are passed
explicitly at the call site, so an `[Arguments]` row must keep passing every value it varies, or two rows collapse
onto the same snapshot file and silently overwrite each other.

- Accept all pending snapshots: `./scripts/accept-all.sh src/EnumGenerator.Tests/snapshots` (renames every `*.received.cs` over `*.verified.cs`).
- DiffEngine: control the diff tool with the `DiffEngine_ToolOrder` env var, or disable it with `DiffEngine_Disable=true`.

### Test framework and the Verify maintenance fee

Tests are TUnit (`[Test]`, `[Arguments(...)]` for parameterized rows, `await Assert.That(x).IsEqualTo(y)` — assertions
are async, so test methods are `async Task`). `Microsoft.NET.Test.Sdk` and `coverlet.*` must **not** come back: they
pull in the VSTest host and break TUnit's test discovery. Each test project is `OutputType=Exe` because MTP generates
the entry point; `src/Directory.Build.targets` mutes CA1515 and CA2007 for test projects, which is fallout from that,
not style drift.

Two things in `EnumTests` are shaped the way they are on purpose:

- `ReadOnlySpan<byte>` has no TUnit assertion — a `ref struct` cannot be a generic type argument — so the `u8`
  comparisons go through `.ToArray()` and `IsEquivalentTo(..., CollectionOrdering.Matching)`. Keep the
  `CollectionOrdering.Matching`: `IsEquivalentTo` ignores order by default. For the same reason the `Throws` cases on
  `AsUtf8Span()` use a statement lambda (`() => { _ = ...; }`) so it binds to `Action` rather than `Func<ReadOnlySpan<byte>>`.
- `RoundTrip` is `async Task` with a separate one-line `unsafe SizeOf<T>()` helper, because `await` is illegal in an
  unsafe context (CS4004) and `sizeof(T)` on an unmanaged type parameter needs one.

xUnit's `Assert.Throws<T>` matches the exact exception type, so these migrated to `ThrowsExactly<T>()`; TUnit's plain
`Throws<T>()` would also accept subclasses.

Verify v33 charges an [Open Source Maintenance Fee](https://github.com/VerifyTests/Verify/blob/main/docs/maintenance-fee.md)
to revenue-generating organizations, enforced at build time. A build referencing any Verify package that declares
nothing fails with **SC021**. This project claims the free `OpenSource` exemption in `src/Directory.Build.props`. The
claim is time-bounded and the build starts failing once `Verify_SponsorshipExemptionUntil` has passed — bump it
yearly; it is not dead config.

Rider needs *Settings → Build, Execution, Deployment → Unit Testing → Testing Platform → "Enable Testing Platform
support"* before it will discover any of these tests.

## Architecture

This is a Roslyn `IIncrementalGenerator` that emits enum helper extension methods. It ships as the `NoahStolk.EnumGenerator` NuGet package, packed by `EnumGenerator.Package` — that project is the only `IsPackable=true` one, and the package contains nothing but the generator DLL in `analyzers/dotnet/cs` (plus the README).

**The package is analyzer-only on purpose — do not add a `lib/` folder to it.** It sets `DevelopmentDependency=true`, which makes NuGet write `<IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>` on install — note the absent `compile`. Anything placed in `lib/` would therefore be invisible to consumers, who would have to hand-edit `IncludeAssets` after every install. This is why the attributes are emitted into the consuming compilation (see below) rather than shipped as a reference assembly. `EnumGenerator.Package` also sets `IncludeBuildOutput=false` (so its own empty assembly stays out of the package) and `SuppressDependenciesWhenPacking=true` (so the empty dependency group does not trip NU5128).

### Project layout

- `EnumGenerator` (netstandard2.0, `IsRoslynComponent=true`) — the incremental generator. Must stay netstandard2.0 for Roslyn host compatibility.
- The attributes are **not** a separate project. `AttributeSourceUtils` holds their source as a string, and `EnumIncrementalGenerator` emits it via `RegisterPostInitializationOutput`. Post-initialization is mandatory here: both providers resolve `[GenerateEnumUtilities]` through the semantic model, and only post-initialization sources are visible to it during the generator run — moving them to `RegisterSourceOutput` would silently break all detection.
- `EnumGenerator.Package` — packaging-only project; produces the NuGet.
- `EnumGenerator.Sample` (net10.0) — debug target for the generator. Use the project's `launchSettings.json` to attach the debugger.
- `EnumGenerator.Tests` (net10.0) — Verify snapshot tests of generator output.
- `EnumGenerator.Tests.Integration` (net10.0) — runtime behavior tests; references the generator as an analyzer (`OutputItemType="Analyzer" ReferenceOutputAssembly="false"`).
- `EnumGenerator.Tests.NuGetIntegration` (net10.0, not in slnx) — same `.cs` files, consumed via packed NuGet.

### Generator pipeline

`EnumIncrementalGenerator` wires up two providers and registers source output for both:

1. **Syntax provider** — looks at every `EnumDeclarationSyntax`, keeps the ones decorated with `[GenerateEnumUtilities]`, and builds an `EnumModel` via `EnumModelBuilder.Build()`.
2. **Compilation provider** — scans `compilation.Assembly` attributes for the generic `GenerateEnumUtilitiesAttribute<T>` (assembly-level, for enums you don't own — e.g. `Silk.NET.OpenGL.BlendEquationModeEXT`). Builds via `EnumModelBuilder.BuildFromCompilation()`.

Both feed into `GenerateEnumUtilities`, which picks between `EnumCodeGenerator` and `FlagsEnumCodeGenerator` based on `EnumModel.HasFlagsAttribute`. Generated files are named `{EnumName}.g.cs`.

`CodeGeneratorUtils` defines the per-method emitters (`GenerateGetIndexMethod`, `GenerateIsDefinedMethod`, etc.) as **C# 14 extension members** on `CodeWriter` (note the `extension(CodeWriter writer) { ... }` block — `LangVersion=14.0` in `Directory.Build.props`). Add new generated methods there and call them from the two code-generator classes.

The default generated class name is `{EnumName}Gen`; consumers can override with `[GenerateEnumUtilities(GeneratedClassName = "...")]` or the generic form's positional/named arg.

### Constraints worth remembering

- **`Microsoft.CodeAnalysis.CSharp` is pinned to 4.14.0 on purpose — do not let a dependency sweep bump it.** The Roslyn version the generator compiles against becomes the *minimum* Roslyn every consumer of the package must have. If it exceeds the version of the `csc` loading it, the compiler refuses the assembly and skips the generator entirely: `warning CS9057: Analyzer assembly ... references version 'X' of the compiler, which is newer than the currently running version 'Y'`. Note CS9057 is only a **warning** — the visible failure is a cascade of `CS1061`/`CS0246` errors for every missing generated member (`ToStringFast`, `HasFlagFast`, …), which makes the real cause easy to miss.
  - 4.14.0 sets the floor at **.NET SDK 9.0.3xx / VS 2022 17.14** (C# 13).
  - Planned bump to **5.0.0** (floor: SDK 10.0.100 / VS 18.0) once .NET 9 hits end of support on **2026-11-10**.
  - To see what a given SDK's compiler is: `dotnet /usr/share/dotnet/sdk/<version>/Roslyn/bincore/csc.dll -version`. SDK 10.0.100 ships Roslyn 5.0; 11.0.100-preview ships 5.7.
  - `EnumGenerator.Tests.NuGetIntegration` is the only test surface that catches this, since it consumes the packed analyzer the way a real consumer does — an in-solution build will not.
- `EnumGenerator.Tests.NuGetIntegration` deliberately declares the **exact** `PrivateAssets`/`IncludeAssets` that `dotnet add package` writes for a development dependency, i.e. **without** `compile`. Do not add assets to make a build pass — if that project stops compiling, the *package layout* is wrong, not the test.
- The generator targets **netstandard2.0**, so APIs like `string.IsNullOrWhiteSpace` are not nullable-annotated — note the `!` suppression in `CodeGeneratorUtils.GetClassName`.
- `EnforceExtendedAnalyzerRules=true` is set on the generator project; avoid APIs that Roslyn flags as unsafe for analyzers/generators.
- Heavy static analysis is on globally (`AnalysisMode=All`, `WarningsAsErrors=nullable`, plus Roslynator, SonarAnalyzer, StyleCop, BannedApiAnalyzers, Nullable.Extended). Expect builds to fail on warnings you'd ignore elsewhere.
- `EnumModel.UniqueMembers` already deduplicates by `ConstantValue` (and filters non-power-of-two values for flags enums) — don't re-filter in generator code; iterate `UniqueMembers` directly.
