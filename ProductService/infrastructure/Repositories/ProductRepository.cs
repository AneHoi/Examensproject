using domain;
using domain.RepositoryInterfaces;

namespace infrastructure.Repositories;

// in memory
public class ProductRepository: IProductRepository
{
    private readonly List<Product> _products;

    public ProductRepository()
    {
        _products = new List<Product>
        {
            new Product
            {
                Id = Guid.NewGuid(), Name = "Shovel", Description = "A shovel for digging", Price = 10.99m,
                Quantity = 100
            },
            new Product
            {
                Id = Guid.NewGuid(), Name = "Hammer", Description = "A hammer for pounding nails", Price = 5.99m,
                Quantity = 50
            },
            new Product
            {
                Id = Guid.NewGuid(), Name = "Saw", Description = "A saw for cutting wood", Price = 15.99m, Quantity = 30
            },
            new Product
            {
                Id = Guid.NewGuid(), Name = "Drill", Description = "A drill for making holes", Price = 25.99m,
                Quantity = 20
            },
            new Product
            {
                Id = Guid.NewGuid(), Name = "Wrench", Description = "A wrench for tightening bolts", Price = 8.99m,
                Quantity = 40
            }
        };
    }

    public Task<Product?> GetProductById(Guid id)
    {
        var product = _products.FirstOrDefault(p => p.Id.Equals(id));
        return Task.FromResult(product);
    }
    
    public Task<IEnumerable<Product>> GetAllProducts()
    {
        return Task.FromResult(_products.AsEnumerable());
    }
}
