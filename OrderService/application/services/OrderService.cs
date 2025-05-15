using contracts;
using domain.interfaces;

namespace application.services;

public class OrderService
{

    private IOrderRepository _orderService;
    public OrderService(IOrderRepository orderService)
    {
        _orderService = orderService;
    }
    
    public bool CreateOrder(CreateOrderDto createOrderDto)
    {
        
        return true;
    }
}