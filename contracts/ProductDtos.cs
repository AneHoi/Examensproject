namespace Contracts;

public record ProductDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public decimal Price { get; init; }
}

public record ProductCreateDto
{
    public string Name { get; init; }
    public string Description { get; init; }
    public decimal Price { get; init; }
}

public record ProductStockCheckRequestDto
{
    public Guid ProductId { get; init; }
    public int QuantityRequired { get; init; }
}

public record ProductStockCheckResponseDto
{
    public Guid ProductId { get; init; }
    public bool IsAvailable { get; init; }
    public decimal QuantityInStock { get; init; }
    public decimal PricePerItem { get; init; }
}

public record UpdateStockRequestDto
{
    public Guid ProductId { get; init; }
    public int QuantityRequired { get; init; }
}