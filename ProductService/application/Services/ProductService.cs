using application.Events;
using Contracts;
using domain;
using domain.RepositoryInterfaces;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;

namespace application;

public class ProductService: IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<ProductService> _logger;
    private readonly IFeatureManager _featureManager;
    
    
    public ProductService(IProductRepository productRepository, ILogger<ProductService> logger, IFeatureManager featureManager)
    {
        _productRepository = productRepository;
        _logger = logger;
        _featureManager = featureManager;
    }
    
    public async Task<ProductDto?> GetProductById(Guid Id)
    {
        _logger.LogInformation("Getting product with ID: {ProductId}", Id);
        
        var product = await _productRepository.GetProductById(Id);
        if (product == null)
        {
            _logger.LogInformation("Product with ID: {ProductId} not found", Id);
            return null;
        }
        var showDiscount = await _featureManager.IsEnabledAsync("show_discounted_price");
        
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = showDiscount ? product.Price * 0.9m : product.Price,
        };
        
    }

    public Task<IEnumerable<ProductDto>> GetAllProducts()
    {
        _logger.LogInformation("Getting all products");
        
        var products = _productRepository.GetAllProducts();
        if (products == null)
        {
            _logger.LogInformation("No products found");
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

    public async Task<ProductDto> CreateProduct(ProductDto productDto)
    {
        _logger.LogInformation("Creating new product: {ProductDto}" , productDto);
        
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = productDto.Name,
            Description = productDto.Description,
            Price = productDto.Price,
        };
        
        var createdProduct = await _productRepository.CreateProduct(product);
        _logger.LogInformation("Created product: {product}", product);
        var evt = new ProductEvent(createdProduct.Id, "Created", DateTime.UtcNow);
        
        
        return new ProductDto
        {
            Id = createdProduct.Id,
            Name = createdProduct.Name,
            Description = createdProduct.Description,
            Price = createdProduct.Price,
        };
    }

    public async Task<ProductDto> UpsertProduct(Guid id, ProductDto productDto)
    {
        _logger.LogInformation("Updating product: {ProductDto}" , productDto);
        var product = new Product
        {
            Id = id,
            Name = productDto.Name,
            Description = productDto.Description,
            Price = productDto.Price,
        };
        
        var updatedProduct = await _productRepository.UpdateProduct(product) 
                             ?? await _productRepository.CreateProduct(product);
        

        return new ProductDto
        {
            Id = updatedProduct.Id,
            Name = updatedProduct.Name,
            Description = updatedProduct.Description,
            Price = updatedProduct.Price,
        };
    }
}
