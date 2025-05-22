namespace domain.RepositoryInterfaces;

public interface IProductRepository
{
    public Task<Product?> GetProductById(Guid id);
    public Task<IEnumerable<Product>> GetAllProducts();
}