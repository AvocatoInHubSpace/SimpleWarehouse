namespace SimpleWarehouse.Core.Domain;

public class MeasureUnit : ArchivedEntity
{
    public Guid Id { get; set; }
    public required string Name { get; init; }
}