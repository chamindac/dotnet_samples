using di_sample.domain.core.Implementation;
using di_sample.domain.core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace di_sample.domain.core.Extenstions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOrganizationServices(
            this IServiceCollection services)
        {
            return services
                .AddSingleton<IOrganizationOperationService, OrganizationOperationService>();
        }
    }
}
