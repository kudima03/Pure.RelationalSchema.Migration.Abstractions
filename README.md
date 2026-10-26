# Pure.RelationalSchema.Migration.Abstractions

Storage-agnostic schema migration abstractions for the **Pure** relational schema ecosystem — source-to-target mappings of schemas, tables and columns.

[![.NET build & check](https://github.com/kudima03/Pure.RelationalSchema.Migration.Abstractions/actions/workflows/build-and-check.yml/badge.svg?branch=main)](https://github.com/kudima03/Pure.RelationalSchema.Migration.Abstractions/actions/workflows/build-and-check.yml)
[![Build and Deploy](https://github.com/kudima03/Pure.RelationalSchema.Migration.Abstractions/actions/workflows/publish-nuget.yml/badge.svg?branch=main)](https://github.com/kudima03/Pure.RelationalSchema.Migration.Abstractions/actions/workflows/publish-nuget.yml)
[![NuGet](https://img.shields.io/nuget/v/Pure.RelationalSchema.Migration.Abstractions)](https://www.nuget.org/packages/Pure.RelationalSchema.Migration.Abstractions)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

## Overview

A schema migration takes the data stored for one version of a relational schema (the *source*) to a changed version of it (the *target*). Unchanged tables and columns map themselves. A renamed or reshaped table, or a renamed or retyped column, needs an explicit mapping to keep its data.

This package holds only the contracts that describe such a migration. It does not depend on any storage package, so a migration can be stored as data (for example as EF Core records in an AOT-compatible domain layer) and handed later to a storage-specific package that carries it out, such as `Pure.RelationalSchema.Storage.PostgreSQL.Migration`.

## Public API

| Name | Namespace | Description |
|------|-----------|-------------|
| `IColumnMapping` | `Pure.RelationalSchema.Migration.Abstractions.ColumnMapping` | `IColumn Source` and the `IColumn Target` that takes over its values |
| `ITableMapping` | `Pure.RelationalSchema.Migration.Abstractions.TableMapping` | `ITable Source`, the `ITable Target` that takes over its rows, and the explicit `IEnumerable<IColumnMapping> Columns` between them |
| `ISchemaMigration` | `Pure.RelationalSchema.Migration.Abstractions.SchemaMigration` | The whole migration as one value: `ISchema Source`, `ISchema Target` and the explicit `IEnumerable<ITableMapping> Tables` |

`Columns` and `Tables` hold the **explicit** mappings only. Consumers map unchanged tables and columns implicitly, so a migration that only adds or drops things can have no mappings at all. How the rest is matched, validated and applied is up to the consuming package.

## Implementing the interfaces

The package ships no implementations: each consumer provides its own, as a plain record or as a persisted entity. A minimal set of records:

```csharp
using Pure.RelationalSchema.Abstractions.Column;
using Pure.RelationalSchema.Abstractions.Schema;
using Pure.RelationalSchema.Abstractions.Table;
using Pure.RelationalSchema.Migration.Abstractions.ColumnMapping;
using Pure.RelationalSchema.Migration.Abstractions.SchemaMigration;
using Pure.RelationalSchema.Migration.Abstractions.TableMapping;

public sealed record ColumnMapping : IColumnMapping
{
    public ColumnMapping(IColumn source, IColumn target)
    {
        Source = source;
        Target = target;
    }

    public IColumn Source { get; }

    public IColumn Target { get; }
}

public sealed record TableMapping : ITableMapping
{
    public TableMapping(ITable source, ITable target, IEnumerable<IColumnMapping> columns)
    {
        Source = source;
        Target = target;
        Columns = columns;
    }

    public ITable Source { get; }

    public ITable Target { get; }

    public IEnumerable<IColumnMapping> Columns { get; }
}

public sealed record SchemaMigration : ISchemaMigration
{
    public SchemaMigration(
        ISchema source,
        ISchema target,
        IEnumerable<ITableMapping> tables
    )
    {
        Source = source;
        Target = target;
        Tables = tables;
    }

    public ISchema Source { get; }

    public ISchema Target { get; }

    public IEnumerable<ITableMapping> Tables { get; }
}
```

Describing a migration in which column `Name` of table `Products` is renamed to `Title`:

```csharp
ISchemaMigration migration = new SchemaMigration(
    source,
    target,
    [new TableMapping(products, renamedProducts, [new ColumnMapping(name, title)])]
);
```

## Design Principles

- **Interfaces-only** — no implementations, no tests; every file defines exactly one interface
- **Storage-agnostic** — depends on the relational schema model only, never on a storage package
- **AOT-compatible** — `IsAotCompatible=true`
- **Documented** — XML doc comments on every public member, shipped in the package

## Dependencies

- [`Pure.RelationalSchema.Abstractions`](https://github.com/kudima03/Pure.RelationalSchema.Abstractions/tree/1.2.0) — `ISchema`, `ITable`, `IColumn` and the other relational schema interfaces

## Target Frameworks

- .NET 8
- .NET 9
- .NET 10

## Installation

```
dotnet add package Pure.RelationalSchema.Migration.Abstractions
```

## Related packages

- `Pure.RelationalSchema.Abstractions` — the relational schema model the mappings refer to
- `Pure.RelationalSchema.Storage.PostgreSQL.Migration` — carries the data stored in PostgreSQL over from the source schema to the target schema
