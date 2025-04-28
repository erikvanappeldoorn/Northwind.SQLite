namespace Northwind.Application;

public class ProductService(IProductRepository repository)
{
   public IEnumerable<(int Index, Product Product)> GetTop3MostExpensiveProducts()
   {
      var products =  repository.GetMostExpensiveProducts();
      var mostExpensiveProducts = products.Take(3);
      
      return mostExpensiveProducts.Select((product, index) => (index+1, product));
   }
}