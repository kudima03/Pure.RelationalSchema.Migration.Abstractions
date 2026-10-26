# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

All `dotnet` commands must be run from the `./src` directory.

```bash
dotnet restore
dotnet build --no-restore -warnaserror
dotnet format --verify-no-changes             # check code style (CI enforces this)
dotnet format                                 # auto-fix code style
dotnet pack --configuration Release -p:Version=<version> --output .
```

CI additionally passes `-p:AssemblyVersion` (pinned to the major) and `-p:FileVersion`; see `.github/workflows/publish-nuget.yml`.

There are no test or benchmark projects. The CI pipeline (`build-and-check.yml`) only builds and checks formatting. Do not run csharpier: its default width of 100 conflicts with the 90-column limit. Its CI step is commented out on purpose, and `dotnet format` is the authoritative style check.

## Architecture

This is an **interfaces-only NuGet library**: no implementations, no tests. Every file defines exactly one interface. It describes a schema migration (source schema, target schema and the explicit mappings between their tables and columns) without depending on any storage package, so that a migration can be stored as data, for example by an AOT-compatible domain layer.

**Interfaces:** one folder and namespace per concept, like `Pure.RelationalSchema.Abstractions` (`.Schema`, `.Table`, `.Column`):

- `ColumnMapping/IColumnMapping`: `IColumn Source`, `IColumn Target`.
- `TableMapping/ITableMapping`: `ITable Source`, `ITable Target`, `IEnumerable<IColumnMapping> Columns`.
- `SchemaMigration/ISchemaMigration`: `ISchema Source`, `ISchema Target`, `IEnumerable<ITableMapping> Tables`.

`Columns` and `Tables` hold the explicit mappings only; consumers map unchanged tables and columns implicitly. Matching, validation and execution belong to the consumers (for example `Pure.RelationalSchema.Storage.PostgreSQL.Migration`), and so do the implementing records. Do not add records or hash types here.

**Dependencies:** only `Pure.RelationalSchema.Abstractions`. Keep its version aligned with what the storage packages resolve, and do not add references to packages that come transitively.

**Documentation:** `GenerateDocumentationFile` is on, so a public type or member without an XML doc comment fails the build (CS1591 under `-warnaserror`). The XML docs ship in the package.

**Multi-targeting:** net8.0, net9.0, net10.0. All interfaces must remain AOT-compatible (`IsAotCompatible=true`).

**Package validation:** `EnablePackageValidation = true`. There is no `PackageValidationBaselineVersion` until the first release. Set it afterwards; breaking API changes then fail the build.

**Publishing:** triggered by pushing a semver tag matching `*.*.*`. The tag name becomes the `PackageVersion`. Packages are published to both GitHub Packages and NuGet.org.

## Code Style

Enforced via `.editorconfig` and `dotnet format --verify-no-changes` in CI:

- No `var`: always use explicit types
- No expression-bodied methods or constructors. Properties, indexers, accessors and lambdas may be expression-bodied
- File-scoped namespace declarations; namespaces match folders
- `using` directives outside the namespace, System directives sorted first
- Max line length: 90 characters
- Braces on new lines (`csharp_new_line_before_open_brace = all`)
- XML doc comments on every public member

## Commit Messages

Conventional Commits (`feat: …`, `fix: …`, `chore: …`, `docs: …`, `ci: …`).
