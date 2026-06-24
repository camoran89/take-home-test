using Fundo.Applications.WebApi;
using Fundo.Applications.WebApi.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Fundo.Services.Tests.Integration
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var projectDir = GetProjectPath();
            builder.UseEnvironment("Development");
            builder.UseContentRoot(projectDir);
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile(Path.Combine(projectDir, "appsettings.json"), optional: false, reloadOnChange: false);
            });

            base.ConfigureWebHost(builder);
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptors = services.Where(d => d.ServiceType == typeof(DbContextOptions<LoanDbContext>)).ToList();
                foreach (var descriptor in descriptors)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<LoanDbContext>(options =>
                {
                    options.UseInMemoryDatabase("LoanManagementTests");
                });
            });

            var host = base.CreateHost(builder);
            InitializeDatabaseAsync(host.Services).GetAwaiter().GetResult();
            return host;
        }

        private static async Task InitializeDatabaseAsync(IServiceProvider services)
        {
            await using var scope = services.CreateAsyncScope();
            var db = scope.ServiceProvider.GetRequiredService<LoanDbContext>();
            await db.Database.EnsureCreatedAsync();
        }

        private static string GetProjectPath()
        {
            var current = Directory.GetCurrentDirectory();
            var directoryInfo = new DirectoryInfo(current);
            while (directoryInfo != null && !Directory.Exists(Path.Combine(directoryInfo.FullName, "Fundo.Applications.WebApi")))
            {
                directoryInfo = directoryInfo.Parent;
            }

            if (directoryInfo == null)
            {
                throw new DirectoryNotFoundException("Could not locate the Fundo.Applications.WebApi project directory from the current test directory.");
            }

            return Path.Combine(directoryInfo.FullName, "Fundo.Applications.WebApi");
        }
    }
}
