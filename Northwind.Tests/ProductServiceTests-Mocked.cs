using FakeItEasy;
using Northwind.Application;

namespace Northwind.Tests;

public class ProductServiceTestsMocked
{
    [Fact]
    public void GetTop3MostExpensiveProductsTests()
    {
        var productRepositoryMock = A.Fake<IProductRepository>();
        A.CallTo(() => productRepositoryMock.GetMostExpensiveProducts()).Returns(new List<Product>
        {
            new(55, "Zaanse Koeken", 3.50),
            new(123, "Stroopwafels", 2.75),
            new(42, "Spritsen", 2.50),
            new(19, "Bokkepootjes", 2.15),
            new(78, "Bastogne koeken", 1.99),
        });
        
        ProductService productService = new ProductService(productRepositoryMock);
        
        var result = productService.GetTop3MostExpensiveProducts().ToList();
        
        Assert.Equal(3, result.Count());
        Assert.Equal(1, result[0].Index);
        Assert.Equal("Stroopwafels", result[1].Product.Name);
        Assert.Equal(3, result[2].Index);
        Assert.True(result[1].Product.Price > result[2].Product.Price);
    }
}