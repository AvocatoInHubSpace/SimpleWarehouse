namespace SimpleWarehouse.Core.Domain;

public class MeasureUnit : ArchivedEntity
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
}