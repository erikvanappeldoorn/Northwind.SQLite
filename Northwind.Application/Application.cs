using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Northwind.Entities;

namespace Northwind.Application;

public class Application(IDbContextFactory<NorthwindContext> northwindContextFactory)
{
    public void ExecuteDemoQuery()
    {
        using var context = northwindContextFactory.CreateDbContext();
        
        var query = context.OrderDetails
            .Include(o => o.Order)
            .GroupBy(o => o.OrderId)
            .Select(group =>
                new
                {
                    OrderId = group.Key,
                    NuberOfOrders = group.Count(),
                    Total = group.Sum(g => g.UnitPrice * g.Quantity)
                })
            .OrderByDescending(o => o.Total)
            .Take(1);

        Console.WriteLine(query.ToQueryString());

        RunQueryWithTiming(() =>
        {
            var result = query.First();

            Console.WriteLine();
            Console.WriteLine("----- Results -----");
            Console.WriteLine();

            Console.WriteLine($"Highest Order id {result.OrderId}. {result.Total:c}");
        });
    }

    public void ExecuteExercise1()
    {
        using var context = northwindContextFactory.CreateDbContext();
        
        var result = from region in context.Regions
                     select region;

        foreach (var region in result)
        {
            Console.WriteLine($"{region.RegionId} - {region.RegionDescription}");
        }
    }
    
    public void ExecuteExercise2()
    {
        using var context = northwindContextFactory.CreateDbContext();

        var result = from employee in context.Employees
            select new
            {
                employee.FirstName,
                employee.LastName,
            };

        foreach (var employee in result)
        {
            Console.WriteLine($"{employee.FirstName} - {employee.LastName}");
        }
    }

    public void ExecuteExercise3()
    {
        using var context = northwindContextFactory.CreateDbContext();

        var result = from employee in context.Employees
            orderby employee.LastName
            select new
            {
                employee.FirstName,
                employee.LastName
            };
        
        foreach (var employee in result)
            Console.WriteLine($"{employee.FirstName} {employee.LastName}");
    }

    public void ExecuteExercise4()
    {
        using var context = northwindContextFactory.CreateDbContext();

        var result = from order in context.Orders
            orderby order.Freight descending
            select new
            {
                order.OrderId,
                order.OrderDate,
                order.ShippedDate,
                order.CustomerId,
                order.Freight,
            };

        Console.WriteLine(result.Count());
        //foreach (var order in result)
        //{
        //    Console.WriteLine($"{order.OrderId} {order.OrderDate} {order.ShippedDate} {order.CustomerId} {order.Freight}");
        //}
    }
    private void RunQueryWithTiming(Action action)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        action();

        stopwatch.Stop();
        Console.WriteLine($"Elapsed time: {stopwatch.ElapsedMilliseconds} ms.", ConsoleColor.Cyan);
    }
}