using Contracts;
using domain.RepositoryInterfaces;

namespace application;

public class ProductService: IProductService
{
    private readonly IProductRepository _productRepository;
    
    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    
    public async Task<ProductDto?> GetProductById(Guid Id)
    {
        var product = await _productRepository.GetProductById(Id);
        if (product == null)
        {
            return null;
        }
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
        };
    }

    public Task<IEnumerable<ProductDto>> GetAllProducts()
    {
        var products = _productRepository.GetAllProducts();
        if (products == null)
        {
            return Task.FromResult(Enumerable.Empty<ProductDto>());
        }
        return Task.FromResult(products.Result.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
        }));
    }
}
