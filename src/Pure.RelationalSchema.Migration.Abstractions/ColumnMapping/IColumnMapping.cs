using Pure.RelationalSchema.Abstractions.Column;

namespace Pure.RelationalSchema.Migration.Abstractions.ColumnMapping;

/// <summary>
/// Maps a column of a source table to the column of the target table that takes over
/// its values, for example when the column is renamed or its type changes.
/// </summary>
public interface IColumnMapping
{
    /// <summary>
    /// The column as it is in the source table.
    /// </summary>
    public IColumn Source { get; }

    /// <summary>
    /// The column as it is in the target table; it receives the values of
    /// <see cref="Source"/>.
    /// </summary>
    public IColumn Target { get; }
}
