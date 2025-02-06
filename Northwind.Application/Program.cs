using Microsoft.Extensions.DependencyInjection;

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
        IServiceCollection services = new ServiceCollection();
        services.AddTransient<Application>();
        
        return services;
    }
}