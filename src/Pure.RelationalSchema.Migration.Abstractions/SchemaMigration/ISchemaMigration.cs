using Pure.RelationalSchema.Abstractions.Schema;
using Pure.RelationalSchema.Migration.Abstractions.TableMapping;

namespace Pure.RelationalSchema.Migration.Abstractions.SchemaMigration;

/// <summary>
/// A whole migration as one value: the schema as it was, the schema as it is now and
/// the explicit mappings between their tables. It describes the migration only;
/// carrying stored data over is up to the consumer.
/// </summary>
public interface ISchemaMigration
{
    /// <summary>
    /// The schema as it was before the migration.
    /// </summary>
    public ISchema Source { get; }

    /// <summary>
    /// The schema as it is after the migration.
    /// </summary>
    public ISchema Target { get; }

    /// <summary>
    /// The explicit mappings between tables of <see cref="Source"/> and tables of
    /// <see cref="Target"/>. Unchanged tables and columns need no mapping: consumers map
    /// them implicitly.
    /// </summary>
    public IEnumerable<ITableMapping> Tables { get; }
}
