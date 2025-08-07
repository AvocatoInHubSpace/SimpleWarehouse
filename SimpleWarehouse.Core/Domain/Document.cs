namespace SimpleWarehouse.Core.Domain;

public class Document
{
    public Guid Id { get; set; }
    public required string Number { get; init; }
    public required DateTime Date { get; init; }
    
    public ICollection<ResourceSupply> ResourceSupplies { get; init; } = new List<ResourceSupply>();
}