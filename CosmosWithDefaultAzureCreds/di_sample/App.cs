using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace di_sample
{
    public class App
    {
        private readonly ILogger<App> _logger;

        public App(ILogger<App> logger)
        {
            _logger = logger;
        }

        public async Task RunAsync()
        {
            _logger.LogInformation("Application started");
            _logger.LogInformation("Hello, Dependency Injection!");
            _logger.LogInformation("Application finished");

            await Task.CompletedTask;
        }
    }
}
