using System;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Microsoft.Azure.Cosmos;

namespace CosmosWithDefaultAzureCreds
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            // Credential class for testing on a local machine or Azure services
            DefaultAzureCredential credential = new(
                new DefaultAzureCredentialOptions
                {
                    TenantId = "tenatid"
                });

            // New instance of CosmosClient class using a connection string
            CosmosClient cosmosClient = new(
                accountEndpoint: "https://ch-px-dev-eus-001-cdb.documents.azure.com:443/",
                tokenCredential: credential
            );

            Database cosmodDb = cosmosClient.GetDatabase("px");
            Container cosmosContainer = cosmodDb.GetContainer("organizations");

            await cosmosContainer.CreateItemAsync(
                item: new
                {
                    id = "test-item-id", // 👈 Now partition is this id field, which is required for Cosmos DB items
                    name = "Test Item",
                    description = "This is a test item created using DefaultAzureCredential.",
                    //partition = "test-item-partion-key", // 👈 This is required
                },
                partitionKey: new PartitionKey("test-item-id") //new PartitionKey("test-item-partion-key")
            );

            cosmosClient.Dispose();
        }
    }
}