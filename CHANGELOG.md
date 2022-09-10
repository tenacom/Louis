# Changelog

All notable changes to L.o.U.I.S. will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## Unreleased changes

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
- **BREAKING CHANGE:** The `Value` method in class `Require` (f.k.a. `Arg` has been renamed to `Of`, as in `Require.Of(value).GreaterThanZero()`.
- **BREAKING CHANGE:** The `ArgHelper` class has been completely revamped and now includes methods that create _and throw_ exceptions, so that calling methods can remain `throw`-less and be better optimized and JITted.
- **BREAKING CHANGE:** The `AsyncWorker` class has been retired from this project. It may reappear in a future version.
- **BREAKING CHANGE:** The `ThreadSafeDisposable` class has been retired from this project, as it brought too little value to be of any actual use.

### Bugs fixed in this release

### Known problems introduced by this release

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
