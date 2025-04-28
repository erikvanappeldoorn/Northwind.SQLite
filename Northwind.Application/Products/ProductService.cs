namespace Northwind.Application;

public class ProductService(IProductRepository repository)
{
   public IEnumerable<(int, Product)> GetTop3MostExpensiveProducts()
   {
      var products =  repository.GetMostExpensiveProducts();
      var mostExpensiveProducts = products.Take(3);
      foreach (var (product, index) in mostExpensiveProducts.Select((product, index) => (product, index)))
      {
         yield return (index+1, product);
      }
   }
}