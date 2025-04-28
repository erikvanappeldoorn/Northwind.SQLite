namespace Northwind.Application;

public static class Extensions
{
    public static Product ToDomain(this Entities.Product product) 
        => new(product.ProductId, product.ProductName, product.UnitPrice);
}