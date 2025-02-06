using System.Reflection;
using Northwind.Application.Repositories;

namespace Northwind.Application;

public class Application
{
    private readonly INorthwindRepository northwindRepository;
    public Application(INorthwindRepository northwindRepository)
    {
        this.northwindRepository = northwindRepository;
    }
    public void Run()
    {
        int numberOfCustomers = northwindRepository.GetCustomers().Count();
        Console.WriteLine($"Number of customers: {numberOfCustomers}");
    }
}