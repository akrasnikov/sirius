using Microsoft.AspNetCore.Identity;
using Serilog;
using ProjectName.Auth.Domain.Entities.Identity;
using ProjectName.Auth.Infrastructure.Seeds;

namespace ProjectName.Auth.WebApi;

public class Program
{
    public async static Task Main(string[] args)
    {
        //Read Configuration from appSettings
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        //Initialize Logger
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(config)
            .WriteTo.Console()
            .CreateLogger();

        var host = CreateHostBuilder(args).Build();
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                var userManager = services.GetRequiredService<UserManager<User>>();
                var roleManager = services.GetRequiredService<RoleManager<Role>>();

                await DefaultRoles.SeedAsync(userManager, roleManager);
                await DefaultSuperAdmin.SeedAsync(userManager, roleManager);
                await DefaultBasicUser.SeedAsync(userManager, roleManager);
                Log.Information("Finished Seeding Default Data");
                Log.Information("Application Starting");
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "An error occurred starting the application");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
        host.Run();
    }
    public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog() //Uses Serilog instead of default .NET Logger
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
