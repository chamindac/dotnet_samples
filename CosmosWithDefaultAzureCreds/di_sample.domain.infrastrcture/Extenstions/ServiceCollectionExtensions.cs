using Azure.Identity;
using di_sample.domain.core.Interfaces.Db;
using di_sample.domain.core.Models.Db;
using di_sample.domain.infrastrcture.Implementation.Db;
using di_sample.domain.infrastrcture.Utils;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.DependencyInjection;

namespace di_sample.domain.infrastrcture.Extenstions
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

        public static IServiceCollection AddOrganizationRepository(
            this IServiceCollection services)
        {
            return services.AddSingleton<
                IGenericDbRepository<OrganizationDbModel>, OrganizationDbRepository>();
        }
    }
}
