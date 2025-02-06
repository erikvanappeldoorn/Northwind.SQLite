using System.Diagnostics;
using System.Net.WebSockets;
using Microsoft.EntityFrameworkCore;
using Northwind.Entities;

namespace Northwind.Application;

public class Application
{
    private readonly NorthwindContext context;
    public Application(NorthwindContext context)
    {
        this.context = context;
    }
    public void Run()
    {
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

    private void RunQueryWithTiming(Action action)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        
        action();
        
        stopwatch.Stop();
        Console.WriteLine($"Elapsed time: {stopwatch.ElapsedMilliseconds} ms.", ConsoleColor.Cyan);
    }
}