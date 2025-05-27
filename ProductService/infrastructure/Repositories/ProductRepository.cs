using domain;
using domain.RepositoryInterfaces;
using Polly;
using Polly.Wrap;
using Polly.Bulkhead;
using Polly.Timeout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly List<Product> _products;
    private readonly AsyncPolicyWrap<Product> _policyWrap;
    private readonly Random _random;

    public ProductRepository()
    {
        _random = new Random();
        
        var retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                3,
                retryAttempt => TimeSpan.FromMilliseconds(200 * retryAttempt),
                (exception, timeSpan, retryCount, context) =>
                {
                    Console.WriteLine($"[RETRY] Attempt {retryCount} due to: {exception.Message}");
                });
        
        var circuitBreakerPolicy = Policy
            .Handle<Exception>()
            .CircuitBreakerAsync(
                3, TimeSpan.FromSeconds(30),
                onBreak: (exception, duration) =>
                {
                    Console.WriteLine($"Circuit Breaker opened for {duration.TotalSeconds} sec due to: {exception.Message}");
                },
                onReset: () => Console.WriteLine("Circuit Breaker Closed"),
                onHalfOpen: () => Console.WriteLine("Circuit Breaker Half-open: testing call"));
        
        var timeoutPolicy = Policy.TimeoutAsync<Product>(TimeSpan.FromSeconds(2),
            TimeoutStrategy.Pessimistic,
            onTimeoutAsync: (context, span, task) =>
            {
                Console.WriteLine($"Timeout {span.TotalSeconds} sec");
                return Task.CompletedTask;
            }
        
        );

        var bulkheadPolicy = Policy.BulkheadAsync<Product>(5, 2,
            onBulkheadRejectedAsync: context =>
            {
                Console.WriteLine($"Bulkhead Rejected");
                return Task.CompletedTask;
            }
        );

        var fallbackProduct = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Fallback Product",
            Description = " Default Fallback Product",
            Price = 0,
            Quantity = 0
        };

        var fallbackPolicy = Policy<Product>
            .Handle<Exception>()
            .FallbackAsync(fallbackProduct,
                onFallbackAsync: (exception, context) =>
                {
                    Console.WriteLine("");
                    return Task.CompletedTask;
                });
        
        _policyWrap = fallbackPolicy.WrapAsync(
            bulkheadPolicy.WrapAsync(
                circuitBreakerPolicy.WrapAsync(
                    retryPolicy.WrapAsync(timeoutPolicy))));

        _products = new List<Product>
        {
            new() { Id = Guid.NewGuid(), Name = "Shovel", Description = "A shovel for digging", Price = 10.99m, Quantity = 100 },
            new() { Id = Guid.NewGuid(), Name = "Hammer", Description = "A hammer for pounding nails", Price = 5.99m, Quantity = 50 },
            new() { Id = Guid.NewGuid(), Name = "Saw", Description = "A saw for cutting wood", Price = 15.99m, Quantity = 30 },
            new() { Id = Guid.NewGuid(), Name = "Drill", Description = "A drill for making holes", Price = 25.99m, Quantity = 20 },
            new() { Id = Guid.NewGuid(), Name = "Wrench", Description = "A wrench for tightening bolts", Price = 8.99m, Quantity = 40 }
        };
    }

    private void MaybeFail()
    {
        if (_random.NextDouble() < 0.3) 
        {
            throw new Exception("Simulated transient failure");
        }
    }

    public async Task<Product?> GetProductById(Guid id)
    {
        return await _policyWrap.ExecuteAsync(async () =>
        {
            MaybeFail();
            var product = _products.FirstOrDefault(p => p.Id == id);
            return await Task.FromResult(product);
        });
    }

    public async Task<IEnumerable<Product>> GetAllProducts()
    { 
        return await Task.FromResult(_products.AsEnumerable());
    }

    public async Task<Product> CreateProduct(Product product)
    {
        return await _policyWrap.ExecuteAsync(async () =>
        {
            MaybeFail();
            _products.Add(product);
            return await Task.FromResult(product);
        });
    }

    public async Task<Product?> UpdateProduct(Product product)
    {
        return await _policyWrap.ExecuteAsync(async () =>
        {
            MaybeFail();
            var existing = _products.FirstOrDefault(p => p.Id == product.Id);
            if (existing == null) return await Task.FromResult<Product?>(null);

            existing.Name = product.Name;
            existing.Description = product.Description;
            existing.Price = product.Price;
            existing.Quantity = product.Quantity;

            return await Task.FromResult<Product?>(existing);
        });
    }
}
