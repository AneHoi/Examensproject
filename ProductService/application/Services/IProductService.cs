using Contracts;
using domain;

namespace application;

public interface IProductService
{
    public Task<ProductDto> GetProductById(Guid Id);
    public Task<IEnumerable<ProductDto>> GetAllProducts();
}