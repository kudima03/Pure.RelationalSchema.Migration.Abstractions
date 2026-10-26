using Pure.RelationalSchema.Abstractions.Table;
using Pure.RelationalSchema.Migration.Abstractions.ColumnMapping;

namespace Pure.RelationalSchema.Migration.Abstractions.TableMapping;

/// <summary>
/// Maps a table of the source schema to the table of the target schema that takes over
/// its rows, for example when the table is renamed or its columns or indexes change.
/// </summary>
public interface ITableMapping
{
    /// <summary>
    /// The table as it is in the source schema.
    /// </summary>
    public ITable Source { get; }

    /// <summary>
    /// The table as it is in the target schema; it receives the rows of
    /// <see cref="Source"/>.
    /// </summary>
    public ITable Target { get; }

    /// <summary>
    /// The explicit mappings between columns of <see cref="Source"/> and columns of
    /// <see cref="Target"/>. Unchanged columns need no mapping: consumers map them
    /// implicitly.
    /// </summary>
    public IEnumerable<IColumnMapping> Columns { get; }
}
