using System.Reflection;
using System.Reflection.Emit;
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
        string relativeDatabasePath = "../../../../northwind.db";
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string databasePath = Path.GetFullPath(baseDirectory + relativeDatabasePath);
        
        if (!File.Exists(databasePath))
            throw new FileNotFoundException($"Database file not found: {databasePath}");
        
        IServiceCollection services = new ServiceCollection();
        services.AddTransient<Application>();
        services.AddDbContext<NorthwindContext>(optionsBuilder =>
        {
            optionsBuilder.UseSqlite($"Data Source={databasePath}");
        });
        return services;
    }
}