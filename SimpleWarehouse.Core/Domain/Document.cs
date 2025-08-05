namespace SimpleWarehouse.Core.Domain;

public class Document
{
    public Guid Id { get; init; }
    public required string Number { get; init; }
    public required DateTime Date { get; init; }
}