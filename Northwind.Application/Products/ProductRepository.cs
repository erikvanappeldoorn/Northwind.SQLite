using Northwind.Entities;

namespace Northwind.Application;

public class ProductRepository(NorthwindContext context) : IProductRepository
{
    public IEnumerable<Product> GetMostExpensiveProducts()
    {
        return context.Products
            .OrderByDescending(product => product.UnitPrice)
            .Take(5)
            .Select(p => p.ToDomain()).ToList();
    }
}