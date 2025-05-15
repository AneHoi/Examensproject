namespace contracts;


public record OrderLineDto
{
    public Guid ProductId { get; init; }
    public int Quantity { get; init; }
    public decimal PricePerItem { get; init; }
    
}

public record CreateOrderDto
{
    public Guid CustomerId { get; init; }
    public IList<OrderLineDto> Items { get; init; } = new List<OrderLineDto>();
}

// Do we need this? it doesnø't include price yet
public record OrderItemRequestDto
{
    public Guid ProductId { get; init; }
    public int Quantity { get; init; }
}


// This will be used to represent a created order (response to client)
public record OrderResponseDto
{
    public Guid Id { get; init; }
    public Guid CustomerId { get; init; }
    public IList<OrderLineDto> Items { get; init; } = new List<OrderLineDto>();
    public DateTime OrderDate { get; init; }
    public decimal TotalAmount { get; init; }
    public string OrderStatus { get; init; } = string.Empty; // Pending, Confirmed, Failed
}

