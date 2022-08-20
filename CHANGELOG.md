# Changelog

All notable changes to L.o.U.I.S. will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## Unreleased changes

### New features

### Changes to existing features

### Bugs fixed in this release

### Known problems introduced by this release

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
