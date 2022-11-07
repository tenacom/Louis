# Exceptions

The `Louis.Diagnostics` namespace helps you follow exception creation and handling best practices.

## (Not) handling critical errors

Critical errors, or critical exceptions as they are called in Microsoft code (see for example [System.Drawing.Common](https://github.com/dotnet/runtime/blob/v6.0.10/src/libraries/System.Drawing.Common/src/System/Drawing/ClientUtils.cs#L16) and [MSBuild](https://github.com/dotnet/msbuild/blob/v17.3.1/src/Shared/ExceptionHandling.cs#L78)) are _exceptions that must not be handled_. They are symptoms of something gone so bad that any attempt to log, warn the user, etc. is likely to cause the program to crash anyway - but with a different exception, so you may never know what went wrong initially.

When a `catch` block receives a critical exception, the best course of action is to do nothing: either rethrow the exception as-is, or better yet, do not catch it in the first place. L.o.U.I.S. makes it as easy as:

```csharp
catch (Exception e) when (!e.IsCriticalError())
{
     // Here you can handle the exception
}
```

Calling `IsCriticalError` in a `when` clause, rather than inside a `catch` block, ensures that critical errors do not cause stack unwinding: they go straight to any outer `catch` block (which hopefully implements the same logic). This is faster (and in this case a lot safer) than catching and rethrowing the same exception.

Exceptions of the following types are considered critical errors:

- [`NullReferenceException`](https://learn.microsoft.com/en-us/dotnet/api/system.nullreferenceexception)
- [`StackOverflowException`](https://learn.microsoft.com/en-us/dotnet/api/system.stackoverflowexception)
- [`AppDomainUnloadedException`](https://learn.microsoft.com/en-us/dotnet/api/system.appdomainunloadedexception)
- [`BadImageFormatException`](https://learn.microsoft.com/en-us/dotnet/api/system.badimageformatexception)
- [`InvalidProgramException`](https://learn.microsoft.com/en-us/dotnet/api/system.invalidprogramexception)
- [`ThreadAbortException`](https://learn.microsoft.com/en-us/dotnet/api/system.threading.threadabortexception)
- [`AccessViolationException`](https://learn.microsoft.com/en-us/dotnet/api/system.accessviolationexception)
- [`OutOfMemoryException`](https://learn.microsoft.com/en-us/dotnet/api/system.outofmemoryexception)
- [`SEHException`](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.sehexception)
- [`SecurityException`](https://learn.microsoft.com/en-us/dotnet/api/system.security.securityexception)

In addition, any exception type that implements the `Louis.Diagnostics.ICriticalError` interface is considered a critical error as well. This allows you to add your own exception types to the list, although the need for it should be rare.

## Drilling down

Given an exception, how do you know whether something went wrong with, say, I/O? An `IOException` may have become the inner exception of a `WhateverLibraryException`, which in turn is the inner exception of one of the inner exceptions of an `AggregateException` that some other library wrapped with a... I think you get the picture by now.

Here comes L.o.U.I.S. to the rescue:

```csharp
catch (Exception e) when (!e.IsCriticalError() && e.AnyCausingException(x => x is IOException))
{
     // Something went wrong with I/O
}
```

If you want to examine the whole set of exceptions represented by an exception, from outer to inner ones, you can get them all as an enumerable:

```csharp
foreach (var e in exception.GetCausingExceptions())
{
    // Do something
}
```

## Rethrowing exceptions

How do you rethrow an exception, preserving stack trace information? Easy: just use [`ExceptionDispatchInfo.Throw(ex)`](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.exceptionservices.exceptiondispatchinfo.throw#system-runtime-exceptionservices-exceptiondispatchinfo-throw(system-exception)) (on .NET or .NET Standard 2.1), or [`ExceptionDispatchInfo.Capture(ex)`](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.exceptionservices.exceptiondispatchinfo.capture)[`.Throw()`](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.exceptionservices.exceptiondispatchinfo.throw#system-runtime-exceptionservices-exceptiondispatchinfo-throw) (on .NET, .NET Framework, or .NET Standard 2.0). Oh, and don't forget to add `System.Runtime.ExceptionServices` to your `using` clauses.

Or just use `ex.Rethrow()`: easier and cleaner, works on any target framework.

## Building exception messages

Sometimes an exception message has to give a clue as to which object caused the exception. But what if the object in question is a 20-megabyte string? You can decide to only tell the object type - a decision you know you'll regret as soon as you're reading a log, trying to divine the value of that `Int32`.

Or you can call `ExceptionHelper.FormatObject(obj)`:

- if `obj` is a null reference, or the default value of a nullable value type, you'll get `<null>`;
- if `obj` is a string, it will be escaped, quoted, and trimmed to a maximum of 43 characters, retaining the first 20 and the last 20 with an ellipsis (three dots) in the middle;
- if `obj` implements `IFormattable` (as all numeric types do, for example), it will be formatted using the invariant culture;
- otherwise, `obj.ToString()` will be called and:
  - if `ToString` fails, you'll get a string like `<{objType}:{exceptionType}>`;
  - if the result is `null`, you'll get `<null!>`;
  - if the result is the empty string, you'll get `<empty!>`.

You may also pass a format string as `FormatObject`'s second parameter. It will be used (with the invariant culture) if `obj` implements `IFormattable`.

If formatting an `IFormattable` instance fails with an exception, `FormatObject` will first retry with an empty format string if it was called with a non-empty one; failing that, it will treat the object as if it didn't implement `IFormattable`.
