using Northwind.Entities;

namespace Northwind.Application.Repositories;

public interface INorthwindRepository
{
    IEnumerable<Customer> GetCustomers();
}