namespace contracts;

public record ProductDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public decimal Price { get; init; }
    public decimal Quantity { get; init; }
}

public record ProductStockCheckRequestDTO
{
    public Guid ProductId { get; init; }
    public Guid QuantityRequired { get; init; }
}

public record ProductStockCheckResponseDTO
{
    public Guid ProductId { get; init; }
    public bool IsAvailable { get; init; }
    public decimal QuantityInStock { get; init; }
    public decimal PricePerItem { get; init; }
    
}

public record UpdateStockRequestDTO
{
    public Guid ProductId { get; init; }
    public Guid QuantityRequired { get; init; }
}
