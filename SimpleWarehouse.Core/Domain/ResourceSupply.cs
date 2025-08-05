namespace SimpleWarehouse.Core.Domain;

public class ResourceSupply
{
    private readonly int _quantity = 1;
    public Guid Id { get; init; }
    public required Guid DocumentId { get; init; }
    public Document? Document { get; init; }
    public required Guid ResourceId { get; init; }
    public Resource? Resource { get; init; }
    public required Guid MeasureUnitId { get; init; }
    public MeasureUnit? MeasureUnit { get; init; }

    public required int Quantity
    {
        get => _quantity;
        init
        {
            if (value < 1)
            {
                throw new ArgumentException("Quantity must be greater than 0");
            }
            _quantity = value;
        }
    }
}