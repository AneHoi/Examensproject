using application.services;
using contracts;
using Microsoft.AspNetCore.Mvc;

namespace presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly ILogger<OrderController> _logger;
    private OrderService _orderService;

    public OrderController(ILogger<OrderController> logger, OrderService orderService)
    {
        _logger = logger;
        _orderService = orderService;
    }

    [HttpPost]
    [Route("Order/createOrder")]
    public OrderItemRequestDto CreateOrder([FromBody] CreateOrderDto createOrderDto)
    {
        bool orderCreated = _orderService.CreateOrder(createOrderDto);
        return null;
    }
}
