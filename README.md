# enum-generator

**🚧 WORK IN PROGRESS 🚧**

[![NuGet Version](https://img.shields.io/nuget/v/NoahStolk.EnumGenerator.svg)](https://www.nuget.org/packages/NoahStolk.EnumGenerator/)

Enum-generator is a zero-dependency library containing a source generator that generates useful extension methods and utilities for enums in C#.

## Examples

TODO

## Benchmarks

TODO

## Development

### Debugging the Source Generator

To debug the source generator, use the `launchSettings.json` file in the `EnumGenerator` project to run the generator against the `EnumGenerator.Sample` project.

You can also debug the generator tests using the `EnumGenerator.Tests` project.

### Snapshot Testing

To control which diff tool is used for snapshot testing, use the `DiffEngine_ToolOrder` environment variable.

In JetBrains Rider, this can be configured under Build, Execution, Deployment > Unit Testing > Test Runner > Environment variables.

You can also disable DiffEngine by setting the `DiffEngine_Disable` environment variable to `true`.

Personally, I use the [Verify Support plugin for Rider](https://plugins.jetbrains.com/plugin/17240-verify-support).
