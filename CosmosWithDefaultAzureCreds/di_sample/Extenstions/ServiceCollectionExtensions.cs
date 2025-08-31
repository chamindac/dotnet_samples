using Azure.Identity;
using di_sample.Utils;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.DependencyInjection;

namespace di_sample.Extenstions
{
    public static class ServiceCollectionExtensions
    {


        public static IServiceCollection AddCosmosClient(
            this IServiceCollection services)
        {
            services.AddSingleton(_ =>
            {
                string cosmosEndpoint = "https://ch-px-dev-eus-001-cdb.documents.azure.com:443/";
                string tenantId = "tenantid";

                DefaultAzureCredential defaultAzureCredential = DefaultAzureCredentialBuilder.Build(tenantId);

                return new CosmosClientBuilder(cosmosEndpoint, defaultAzureCredential)
                            .WithConnectionModeDirect()
                            .Build();
            });

            return services;
        }
    }
}
