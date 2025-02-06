using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Northwind.Entities;

namespace Northwind.Application;

class Program
{
    static void Main(string[] args)
    {
        var services = ConfigureServices();
        var serviceProvider = services.BuildServiceProvider();
        var application = serviceProvider.GetService<Application>();
        application?.Run();
    }

    private static IServiceCollection ConfigureServices()
    {
        const string databasePath =  @"/Users/erikvanappeldoorn/Projects/Northwind.SqlLite/Northwind.Entities/northwind.db";
        
        IServiceCollection services = new ServiceCollection();
        services.AddTransient<Application>();
        services.AddDbContext<NorthwindContext>(optionsBuilder => optionsBuilder.UseSqlite($"Data Source={databasePath}"));
        return services;
    }
}