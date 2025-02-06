using Northwind.Entities;

namespace Northwind.Application.Repositories;

public class NorthwindRepository : INorthwindRepository
{
    public IEnumerable<Customer> GetCustomers()
    {
        using var context = new NorthwindContext();
        return context.Customers.ToList();
    }
}