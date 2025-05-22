using contracts;

namespace domain.interfaces;

public interface IOrderRepository
{
    bool CreateOrder(CreateOrderDto createOrderDto);
}