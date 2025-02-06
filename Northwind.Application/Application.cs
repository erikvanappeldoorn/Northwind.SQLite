using System.Diagnostics;
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
        RunQueryWithDiagnostics(() =>
        {
            var orderInfo = context.OrderDetails
                .Include(o => o.Order)
                .GroupBy(o => o.OrderId)
                .OrderBy(group => group.Key)
                .Select(group =>
                    new
                    {
                        OrderId = group.Key,
                        NuberOfOrders = group.Count(),
                        Total = group.Sum(g => g.UnitPrice * g.Quantity)
                    }).ToList();

            var highestOrder = orderInfo.OrderByDescending(o => o.Total).First();

            Console.WriteLine($"Highest Order id {highestOrder.OrderId}. {highestOrder.Total:c}");
        });
    }

    private void RunQueryWithDiagnostics(Action query)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        
        query();
        
        stopwatch.Stop();
        Console.WriteLine($"Elapsed time: {stopwatch.ElapsedMilliseconds} ms.");
    }
}