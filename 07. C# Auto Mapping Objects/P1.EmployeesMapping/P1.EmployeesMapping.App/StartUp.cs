namespace P1.EmployeesMapping.App
{
    using System;
    using P1.EmployeesMapping.Data;
    using P1.EmployeesMapping.Model;
    //using P1.EmployeesMapping.Services; 
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.EntityFrameworkCore;
    using global::EmployeesMapping.Services;
    using AutoMapper;

    //using global::EmployeesMapping.Services;

    public class StartUp
    {
        static void Main()
        {
            var serviceProvider = ConfigureServices();
            var engine = new Engine(serviceProvider);
            engine.Run();
        }

        static IServiceProvider ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<EmployeesMappingContext>(options =>
                options.UseSqlServer(Configuration.ConnectionString));

            serviceCollection.AddTransient<EmployeeService>();

            serviceCollection.AddAutoMapper(cfg =>
            cfg.AddProfile<AutomapperProfile>()); 

            var serviceProvider = serviceCollection.BuildServiceProvider();

            return serviceProvider;
        }
    }
}
