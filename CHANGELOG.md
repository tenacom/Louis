# Changelog

All notable changes to L.o.U.I.S. will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## Unreleased changes

### New features

- Structs `Louis.Text.TrueConditionInterpolatedStringHandler` and `Louis.Text.FalseConditionInterpolatedStringHandler` are interpolated string handlers that only perform formatting if a given condition is true or false, respectively.  
They can help implement, for example, conditional logging methods that take a condition and an interpolated string as a parameter, ensuring that string interpolation will only be performed if the result is actually used.

### Changes to existing features

### Bugs fixed in this release

### Known problems introduced by this release

## [2.0.31](https://github.com/Tenacom/Louis/releases/tag/2.0.31) (2024-02-26)

### New features

- Class `Louis.ComponentModel.ParsableStringConverter<T>` and method `Louis.ComponentModel.SimpleStringConverter.AddToTypeDescriptor<T>` offer a ready-made type converter for any type implementing [`IParsable<TSelf>`](https://learn.microsoft.com/en-us/dotnet/api/system.iparsable-1).  
`ParsableStringConverter<T>` and `AddToTypeDescriptor<T>` are only available on target platforms where `IParsable<TSelf>` is available, i.e. .NET 7 and later versions.
- Two new boolean properties in class `Louis.Hosting.AsyncHostedService` let subclasses decide whether `StartAsync` should fail when the service is stopped before starting (`FailOnSetupNotStarted`) or `SetupAsync` completes with `false` (`FailOnSetupUnsuccessful`).  
The default value is `true` for both properties.
- Struct `Louis.Threading.InterlockedFlag` now implements `IEquatable<bool>`, as well as equality and inequality operators with `bool`.
- New struct `Louis.Threading.InterlockedReference<T>` encapsulates an object reference, so that it is always accessed in a thread-safe fashion.
- New class `Louis.IO.ReadOnlyMemoryStream` implements a read-only, seekable `Stream` backed by a `ReadOnlyMemory<byte>`.

### Changes to existing features

- **BREAKING CHANGE:** In class `Louis.Threading.AsyncService`, virtual method `SetupAsync` now returns a `ValueTask<bool>` instead of a `ValueTask`. If the result of the task is `false`, the service is stopped and neither `ExecuteAsync` nor `TeardownAsync` are called.
- **BREAKING CHANGE:** In class `Louis.Threading.AsyncService`, the tasks returned from methods `WaitUntilStartedAsync` and `StartAndWaitAsync` now have a result of type `AsyncServiceSetupResult`, with the following meaning:
  - `AsyncServiceSetupResult.Successful` means that `SetupAsync` completed with a `true` result;
  - `AsyncServiceSetupResult.NotStarted` means that the service was stopped before being started and `SetupAsync` was therefore not called;
  - `AsyncServiceSetupResult.Unsuccessful` means that `SetupAsync` completed with a `false` result;
  - `AsyncServiceSetupResult.Canceled` means that `SetupAsync` was canceled;
  - `AsyncServiceSetupResult.Faulted` means that `SetupAsync` threw an exception.
- **BREAKING CHANGE:** In class `Louis.Threading.AsyncService`, methods `StartAsync` and `StopAsync` have been renamed to `StartAndWaitAsync` and `StopAndWaitAsync`, respectively. The old names lead some users (and code analysis tools, e.g. ReSharper) to believe they were asynchronous versions of `Start` and `Stop`.
- Class `Louis.Hosting.AsyncHostedService` now explicitly implements the `StartAsync` and `StopAsync` methods from `IHostedService`.  
The two methods were previously only visible when casting an instance to `IHostedService`, to avoid confusion with methods inherited from `Luois.Threading.AsyncService`. However, this violated design rule [CA1033](https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1033).

## [1.3.4](https://github.com/Tenacom/Louis/releases/tag/1.3.4) (2023-11-26)

### New features

- Class `Louis.ActionDisposable` implements [`IDisposable`](https://learn.microsoft.com/en-us/dotnet/api/system.idisposable) by invoking an [`Action`](https://learn.microsoft.com/en-us/dotnet/api/system.action) passed to its constructor. This can be useful, combined with C#'s [`using` statement](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/using), to ensure a piece of code gets executed at the end of a block or method, regardless of its result or outcome.
- `Louis.LocalActionDisposable` has the same purpose and API of `ActionDisposable` but, being a [`ref struct`](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/ref-struct), it cannot be passed outside the method it is created in. On the other hand, it doesn't allocate space on the heap, taking just the size of a pointer on the stack, so it is preferrable to `ActionDisposable` in most cases, from both a performance and memory pressure point of view.
- Class `Louis.AsyncActionDisposable` is similar to `ActionDisposable`, but it takes a possibly asynchronous delegate and uses it to implement [`IAsyncDisposable`](https://learn.microsoft.com/en-us/dotnet/api/system.iasyncdisposable) as well as [`IDisposable`](https://learn.microsoft.com/en-us/dotnet/api/system.idisposable).

## [1.2.3](https://github.com/Tenacom/Louis/releases/tag/1.2.3) (2023-11-20)

### New features

- .NET 8.0 has been added as a target framework.

## [1.1.12](https://github.com/Tenacom/Louis/releases/tag/1.1.12) (2023-11-09)

### New features

- Class `Louis.ComponentModel.SimpleStringConverter<T>` provides a base class for type converters that can convert a specific type to and/or from a string.
This abstract class takes care of boilerplate code and dealing with `object`s; subclasses only have to implement conversions between `string`s and strongly-typed instances.
- Static method `Louis.ComponentModel.SimpleStringConverter.AddToTypeDescriptor<T, TConverter>` creates an instance of [`TypeConverterAttribute`](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.typeconverterattribute) referencing a subclass of `SimpleStringConverter<T>` and registers it for use by [`TypeDescriptor.GetAttributes`](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.typedescriptor.getattributes). This enables a converter to be recognized by e.g. [`ConfigurationBinder`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.configuration.configurationbinder) with just one line of clean, easy-to-understand code.
- Classes `Louis.ComponentModel.MailAddressConverter` and `Louis.ComponentModel.MailAddressCollectionConverter` perform conversion of [`MailAddress`](https://learn.microsoft.com/en-us/dotnet/api/system.net.mail.mailaddress) and [`MailAddressCollection`](https://learn.microsoft.com/en-us/dotnet/api/system.net.mail.mailaddresscollection), respectively, to and from `string`. They are also good examples of how to subclass `SimpleStringConverter<T>`.
- New fluent extension method `IfNotNullOrEmpty` invokes either an `Action` or a `FluentAction` if a string is neither null nor the empty string.
- Fluent extension method `IfNotNull` now has overloads that work with nullable value types.

## [1.0.175](https://github.com/Tenacom/Louis/releases/tag/1.0.175) (2023-09-27)

First stable version. No actual changes since last preview.

## [1.0.173-preview](https://github.com/Tenacom/Louis/releases/tag/1.0.173-preview) (2023-09-26)

### Bugs fixed in this release

- Some methods in date- and time-related utilities, which took instances of `CultureInfo` or `DateTimeFormatInfo` as parameters, did not check for `null` arguments. This has been fixed and `ArgumentNullException` is now thrown when needed.

## [1.0.170-preview](https://github.com/Tenacom/Louis/releases/tag/1.0.170-preview) (2023-09-26)

### New features

- [#83 - Add date- and time-related utilities and extensions](https://github.com/Tenacom/Louis/pull/83). The following types were added (links point to online API reference): [TimeConstants](https://tenacom.github.io/Louis/api/Louis.TimeConstants.html), [DateUtility](https://tenacom.github.io/Louis/api/Louis.DateUtility.html), [DateOnlyExtensions](https://tenacom.github.io/Louis/api/Louis.DateOnlyExtensions.html), [DateTimeExtensions](https://tenacom.github.io/Louis/api/Louis.DateTimeExtensions.html), and [DateTimeUtility](https://tenacom.github.io/Louis/api/Louis.DateTimeUtility.html).

### Changes to existing features

- [#79 - Remove the Louis.Logging library](https://github.com/Tenacom/Louis/issues/79)

## [1.0.152-preview](https://github.com/Tenacom/Louis/releases/tag/1.0.152-preview) (2023-08-13)

### Changes to existing features

- `Louis.Fluency.FluentExtensions.Switch` has new overloads that let you specify an [`IEqualityComparer<T>`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.iequalitycomparer-1) interface to compare the given value with comparands associated with actions.
- Pre-existing overloads of `Louis.Fluency.FluentExtensions.Switch` now use [`EqualityComparer<T>.Default`](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.equalitycomparer-1.default). As a consequance, the value and comparands no longer have to implement `IEquatable<T>`. This change also makes `FluentExtensions.Switch` usable with `enum`s.

## [1.0.148-preview](https://github.com/Tenacom/Louis/releases/tag/1.0.148-preview) (2023-08-01)

### New features

- Added `Louis.Fluency.FluentExtensions.Chain` method group, semantically equivalent to `Invoke` but using fluent methods or lambdas that return the same type as their first parameters. Now you can use even a local function as if it were an extension method.

## [1.0.138-preview](https://github.com/Tenacom/Louis/releases/tag/1.0.138-preview) (2023-07-23)

### New features

- Added some more logging hooks to `AsyncService`. `AsyncHostedService` of course overrides all of them and logs appropriately.
- `AsyncHostedService` now also logs when it is started / stopped by the host.

## [1.0.131-preview](https://github.com/Tenacom/Louis/releases/tag/1.0.131-preview) (2023-07-07)

### New features

- Added class `Louis.Threading.AsyncService`: a complete revamp of the old `AsyncWorker` class that was present in the very first alpha version of L.o.U.I.S., this class simplifies the implementation and use of long-running background tasks.
- Added package `Louis.Hosting` with an `AsyncHostedService` class, that extends `AsyncService` with logging and implements the [`IHostedService`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.hosting.ihostedservice) interface for integration in ASP.NET applications, as well as any application based on the [`generic host`](https://learn.microsoft.com/en-us/dotnet/core/extensions/generic-host).

## [1.0.101-preview](https://github.com/Tenacom/Louis/releases/tag/1.0.101-preview) (2023-06-25)

### New features

- Added overloads of `Louis.Fluency.FluentExtensions.Invoke` that allow for additional arguments to be passed to lambdas to avoid the allocation of closure objects.
- Added method `Louis.Fluency.FluentExtensions.InvokeIf` that calls the provided action only if a condition is `true`.

## [1.0.93-preview](https://github.com/Tenacom/Louis/releases/tag/1.0.93-preview) (2023-05-28)

### New features

- Added method `Louis.Fluency.FluentExtensions.IfNotNull`, which calls an action on an object and an additional argument only if the latter is not `null`.
- Added overloads to `Louis.Fluency.FluentExtensions` that take simple `Action`s instead of `FluentAction`s as arguments.

## [1.0.83-preview](https://github.com/Tenacom/Louis/releases/tag/1.0.83-preview) (2023-04-25)

### New features

- Added method `Louis.Collections.EnumerableExtensions.WhereNot`, which works like `System.Linq.Enumerable.Where` but reverses the meaning of the predicate, returning only elements for which it returns `false`.
- Added method `Louis.Collections.EnumerableExtensions.WhereNotNulLOrEmpty`, that filters out null and empty elements from sequences of strings.
- Added method `Louis.Collections.EnumerableExtensions.WhereNotNulLOrWhiteSpace`, that filters out null, empty, and white-space-only elements from sequences of strings.

### Changes to existing features

- Both overloads of `Louis.Collections.EnumerableExtensions.WhereNotNull` have been rewritten using local static functions instead of lambdas. Generated IL fornth ese methods is now smaller, slightly more performant, and makes no allocations.

### Bugs fixed in this release

- The overload of `Louis.Collections.EnumerableExtensions.WhereNotNull` that accepts sequences of nullable value types was not really an extension method (its first parameter had no `this` modifier).

## [1.0.73-preview](https://github.com/Tenacom/Louis/releases/tag/1.0.73-preview) (2023-03-12)

### New features

- Added class `PrefixingLogger` (in the `Louis.Logging` package), that wraps an existing `ILogger` adding a given prefix to all log messages. Nice work by @ric15ni in [PR #27](https://github.com/Tenacom/Louis/pull/27).

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
