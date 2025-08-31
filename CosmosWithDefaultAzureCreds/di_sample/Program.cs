using di_sample.domain.core.Extenstions;
using di_sample.domain.infrastrcture.Extenstions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace di_sample
{ 
    class Program
    {
        static async Task Main(string[] args)
        {
            ServiceCollection services = new();

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSimpleConsole(options =>
                {
                    options.IncludeScopes = true;
                });
            });

            ConfigureServices(services);

            using (var serviceProvider = services.BuildServiceProvider())
            {
                App app = serviceProvider.GetRequiredService<App>();
                await app.RunAsync();
            }
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<App>()
                .AddCosmosClient()
                .AddOrganizationRepository()
                .AddOrganizationServices();
        }
    }
}