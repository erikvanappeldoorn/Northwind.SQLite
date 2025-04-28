namespace Northwind.Application;

public interface IProductRepository
{
    IEnumerable<Product> GetMostExpensiveProducts();
}