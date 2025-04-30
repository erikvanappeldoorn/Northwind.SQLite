using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Northwind.Application;
using Northwind.Entities;
using Product = Northwind.Entities.Product;

namespace Northwind.Tests;

public class ProductServiceTests_InMemoryDB
{
    [Fact]
    public void GetTop3MostExpensiveProductsTests()
    {
        var contextOptions = new DbContextOptionsBuilder<NorthwindContext>()
            .UseInMemoryDatabase("NorthwindTest")
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        using var context = new NorthwindContext(contextOptions);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        context.AddRange(
            new Product {ProductId = 19, ProductName = "Bokkepootjes", UnitPrice = 2.15, Discontinued = "0"},
            new Product {ProductId = 42, ProductName = "Spritsen", UnitPrice = 2.50, Discontinued = "0"},
            new Product {ProductId = 55, ProductName = "Zaanse Koeken", UnitPrice = 3.50, Discontinued = "0"},
            new Product {ProductId = 78, ProductName = "Bastogne koeken", UnitPrice = 1.99, Discontinued = "0"},
            new Product {ProductId = 123, ProductName = "Stroopwafels", UnitPrice = 2.75, Discontinued = "0"}
        );

        context.SaveChanges();
        
        var productRepository = new ProductRepository(context);
        ProductService productService = new(productRepository);
        
        var result = productService.GetTop3MostExpensiveProducts().ToList();
        
        Assert.Equal(3, result.Count);
        Assert.Equal(1, result[0].Index);
        Assert.Equal("Stroopwafels", result[1].Product.Name);
        Assert.Equal(3, result[2].Index);
        Assert.True(result[1].Product.Price > result[2].Product.Price);
    }
}