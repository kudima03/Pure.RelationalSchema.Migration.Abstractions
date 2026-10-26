# Changelog

All notable changes to `Pure.RelationalSchema.Migration.Abstractions` are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [0.1.0-preview.0.1.0] - 2026-10-26

### Added

- `IColumnMapping` and `ITableMapping`, moved from `Pure.RelationalSchema.Storage.PostgreSQL.Migration` so that migrations can be described without a storage dependency.
- `ISchemaMigration`: a whole migration as one value, made of the source schema, the target schema and the explicit table mappings.
