# Changelog

All notable changes to L.o.U.I.S. will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## Unreleased changes

### New features

- Added class `PrefixingLogger` (in the `Louis.Logging` package), that wraps an existing `ILogger` adding a given prefix to all log messages. Nice work by @ric15ni in [PR #27](https://github.com/Tenacom/Louis/pull/27).

### Changes to existing features

### Bugs fixed in this release

### Known problems introduced by this release

## [1.0.59-preview](https://github.com/Tenacom/Louis/releases/tag/1.0.59-preview) (2023-03-10)

### Bugs fixed in this release

- [#24 - Log level checking in interpolated string handlers is wrong](https://github.com/Tenacom/Louis/issues/24)

## [1.0.47-preview](https://github.com/Tenacom/Louis/releases/tag/1.0.47-preview) (2022-12-02)

### Changes to existing features

- A rough capacity check has been added to all `StringBuilder` extension methods that append quoted strings. Computing the exact needed capacity, although possible, would totally kill performance; the added checks are a compromise that still helps, especially when appending long strings to string builders with no or little available buffer space.

## [1.0.39-preview](https://github.com/Tenacom/Louis/releases/tag/1.0.39-preview) (2022-11-26)

This version uses completely revamped build scripts and workflows.

As part of the transition to the new build system, versioning is now managed with [`Nerdbank.GitVersioning`](https://github.com/dotnet/Nerdbank.GitVersioning), hence the versioning scheme change.

### New features

- .NET 7 has been added as a target platform.
- The new `DisposeSynchronously()` extension method for the `IAsyncDisposable` interface lets you implement `Dispose()` in a class where you already have a `DisposeAsync()` implementation. Calling `DisposeAsync().GetAwaiter().GetResult()` is the same as calling `DisposeSynchronously()`, but triggers warning `CA2012` ("UseValueTask correctly"); besides, `DisposeSynchronously()` better conveys intent, making code more readable.
- A new overload of `ValueTaskUtility.WhenAll()` takes a variable number of `ValueTask` parameters.

### Changes to existing features

- **BREAKING CHANGE:** Following .NET's [Library support for older frameworks](https://learn.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/7.0/old-framework-support) policy, support for .NET Core 3.1 has been removed.
- **BREAKING CHANGE:** The `Louis.Logging` namespace has been moved to its own library. Therefore, `Louis.dll` no longer depends on `Microsoft.Extensions.Logging.Abstractions.dll`; the new `Louis.Logging.dll` of course does.
- The algorithm used by `ExceptionHelper.FormatObject` has changed as follows:
  - any exception thrown while trying to format an instance of `IFormattable` causes a fallback (previously, exceptions other than `FormatException` were not caught);
  - if formatting an `IFormattable` with an empty format causes an exception, the fallback action is now to treat the object as non-formattable (previously, the string `<invalid_format>` was returned);
  - an exception thrown by `obj.ToString()` causes a string like `<{objTypeName}:{exceptionTypeName}>` to be returned (previously, only the exception type name was specified in the returned string).
- **BREAKING CHANGE:** `Louis.dll` no longer provides polyfills. Instead, it uses the [PolyKit](https://github.com/Tenacom/PolyKit) package. Projects that relied on polyfills provided by L.o.U.I.S. now should add a `PackageReference` to `PolyKit`.
- **BREAKING CHANGE:** Class `Louis.Diagnostics.Throw` and the whole `Louis.ArgumentValidation` namespace have been removed. L.o.U.I.S. now relies on the [`CommunityToolkit.Diagnostics`](https://github.com/CommunityToolkit/dotnet#readme) package for throw helpers and argument validation. This change frees up development resources by eliminating the need to maintain features that, since [the release of the .NET Community Toolkit 8.0](https://devblogs.microsoft.com/dotnet/announcing-the-dotnet-community-toolkit-800/), didn't add much value to begin with.
- **BREAKING CHANGE:** All extension methods in `Louis.Text` that generate clipped string literals (`StringExtensions.ToClippedLiteral`, `StringBuilderExtensions.AppendClippedLiteral`, etc.) now throw `ArgumentOutOfRangeException` if the `headLength` and/or `tailLength` parameter is a negative number. Previously, negative head / tail lengths were treated as 0.
- **BREAKING CHANGE:** Methods `DisposingUtility.DisposeAll` and `DisposingUtility.DisposeAllAsync` have been renamed to `Dispose` and `DisposeAsync` respectively, so they are now overloads of the single-object `Dispose` and `DisposeAsync`.

### Bugs fixed in this release

- Passing incorrect parameter values to most public-facing methods could previously result in confusing exception messages. This has been fixed by implementing parameter checking in all public-facing methods instead of relying on dependency / runtime methods to fail.
- Calling `EnumerableExtensions.DisposeAll` or `DisposingUtility.DisposeAll` from a UI thread could result in failures due to the loss of synchronization context. Tjhis has been fixed.

## [1.0.0-preview.8](https://github.com/Tenacom/Louis/releases/tag/1.0.0-preview.8) (2022-09-13)

### Bugs fixed in this release

- Due to a logic bug, the `Louis.Text.Utf8Utility.GetMaxCharsInBytes` method returned incorrect results. This has been fixed.

## [1.0.0-preview.7](https://github.com/Tenacom/Louis/releases/tag/1.0.0-preview.7) (2022-09-13)

### New features

- New extension methods for `string`, `ReadOnlySpan<char>`, and `StringBuilder` can convert a string (or span) to a C# literal while clipping long strings, leaving a head and/or a tail and an ellipsis. Especially useful for logging and exception messages.
- Method `Louis.Diagnostics.ExceptionHelper.FormatObject` returns a text representation for an object, suitable for inclusion in an exception message.
- Extension method `Louis.Diagnostics.StringBuilderExtensions.AppendFormattedObject` appends a text representation for an object, suitable for inclusion in an exception message, to the end of a `StringBuilder`.
- The `Louis.RangeCheck` class provides methods for easy in-range verification and clamping, with or without custom comparers.
- The new `Validated` class provides methods for faster argument validation than `Require`, when only a simple non-nullability check is needed. Methods of `Validated` do not initiate validation chains, but are faster and consume less stack than their namesakes in `Require`.  
More importantly, `Validated.NotNull` can be used when the type of the checked parameter is an open generic type with neither a `class` nor a `struct` constraint. `Require.NotNull` would not work in this case, because the compiler could not resolve the ambiguity between overloads.
- New `Throw.Aggregate` and `Throw.Aggregate<T>` helper methods for throwing `AggregateException`s.

### Changes to existing features

- **BREAKING CHANGE:** The `Arg` class (in namespace `Louis.ArgumentValidation`) has been renamed to `Require` to make its intent clearer, as e.g. in `Require.NotNull(str)`.
- **BREAKING CHANGE:** The `Value` method in class `Require` (f.k.a. `Arg`) has been renamed to `Of`, as in `Require.Of(value).GreaterThanZero()`.
- **BREAKING CHANGE:** The `ArgHelper` class has been completely revamped and now includes methods that create _and throw_ exceptions, so that calling methods can remain `throw`-less and be better optimized and JITted.
- **BREAKING CHANGE:** The `AsyncWorker` class has been retired from this project. It may reappear in a future version.
- **BREAKING CHANGE:** The `ThreadSafeDisposable` class has been retired from this project, as it brought too little value to be of any actual use.
- **BREAKING CHANGE:** The `ArgExtensions` class, containing methods to check `string` arguments for conformance to URI / URL formats, has been retired. Its methods may reappear in a future version, probably in a different form.
- All overloads of the `AppendQuotedClippedLiteral`, `AppendVerbatimClippedLiteral`, and `AppendClippedLiteral` methods (in class `Louis.Text.StringBuilderExtensions`) have been split in two versions (with and without the `useUnicodeEllipsis` parameter) instead of having an optional parameter. This change minimizes the chance of binary incompatibilities with future versions of L.o.U.I.S.

## [1.0.0-preview.6](https://github.com/Tenacom/Louis/releases/tag/1.0.0-preview.6) (2022-08-24)

### New features

- The `Louis.Logging` namespace contains `ILogger` extensions to log using [interpolated strings](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/tokens/interpolated) instead of separated message formats and arguments.
Not as fast as [`LoggerMessage`](https://docs.microsoft.com/en-us/dotnet/core/extensions/high-performance-logging)-created delegates, but still pretty fast. If the desired log level is not enabled on a logger, parameter evaluation and string interpolation don't happen at all; most importantly, you don't have to disrupt your flow to create a partial method every time you want to add a log write.  
The new overloads have a single drawback: since their custom string interpolation handler uses thread-static `StringBuilder`s, you cannot use `await` in interpolation expressions. If you do, very bad things can and will happen, from garbled log messages to apparently random exceptions, including null pointer exceptions.
- Also in `Louis.Logging`, more `ILogger` extensions to log constant strings without turning them into zero-argument templates.
Besides the small performance advantage, these extensions avoid putting constant log messages in the global [log formatter cache](https://github.com/dotnet/runtime/blob/v6.0.8/src/libraries/Microsoft.Extensions.Logging.Abstractions/src/FormattedLogValues.cs#L18).

## [1.0.0-preview.5](https://github.com/Tenacom/Louis/releases/tag/1.0.0-preview.5) (2022-08-23)

### New features

- `Louis.Text.UnicodeCharacterUtility` is a slightly modified version of an internal class of the Roslyn compiler.
Its methods provide the same functionality as
[SyntaxFacts.IsIdentifierStartCharacter](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntaxfacts.isidentifierstartcharacter),
[SyntaxFacts.IsIdentifierPartCharacter](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntaxfacts.isidentifierpartcharacter), and
[SyntaxFacts.IsValidIdentifier](https://docs.microsoft.com/en-us/dotnet/api/microsoft.codeanalysis.csharp.syntaxfacts.isvalididentifier),
plus an overload of the latter that takes a read-only span instead of a string.

## [1.0.0-preview.4](https://github.com/Tenacom/Louis/releases/tag/1.0.0-preview.4) (2022-08-20)

### Changes to existing features

- Type parameters `T1` and `T2` in `Louis.Fluency.FluentAction<T,T1>` and `Louis.Fluency.FluentAction<T,T1,T2>` delegates are no longer contravariant.  
Contravariance lead to the need for more verbose lambda syntax when `T1` and/or `T2` was a value type. For example, given a `StringBuilder builder` and a `byte[] bytes`,
to concatenate the hexadecimal representations of all bytes in the array you would now write `builder.ForEach(bytes, (sb, b) => sb.Append(b.ToString("x2")))`, whereas
in previous versions of Louis you had to write the same code as `builder.ForEach(bytes, (StringBuilder sb, in byte b) => sb.Append(b.ToString("x2")))`.

## [1.0.0-preview.3](https://github.com/Tenacom/Louis/releases/tag/1.0.0-preview.3) (2022-08-20)

### New features

- Two new overloads of `Louis.Fluency.FluentExtensions.ForEach` allow to iterate over spans instead of enumerables.

### Bugs fixed in this release

- XML documentation for generic methods of class `Louis.Diagnostics.Throw` lacked a description of their type parameter.

## [1.0.0-preview.2](https://github.com/Tenacom/Louis/releases/tag/1.0.0-preview.2) (2022-08-17)

### Bugs fixed in this release

- Method `AsyncWorker.StartAsync` tried to start an already started Task, resulting in an `InvalidOperationException`.

## [1.0.0-preview.1](https://github.com/Tenacom/Louis/releases/tag/1.0.0-preview.1) (2022-08-15)

Initial release.
