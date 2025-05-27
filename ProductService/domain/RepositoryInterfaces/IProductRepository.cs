namespace domain.RepositoryInterfaces;

public interface IProductRepository
{
    public Task<Product?> GetProductById(Guid id);
    public Task<IEnumerable<Product>> GetAllProducts();
    public Task<Product> CreateProduct(Product product);
    Task<Product?> UpdateProduct(Product product);
}