namespace SimpleWarehouse.Core.Domain;

public class Resource : ArchivedEntity
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
}