# Changelog

All notable changes to L.o.U.I.S. will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## Unreleased changes

### New features

### Changes to existing features

### Bugs fixed in this release

### Known problems introduced by this release

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
