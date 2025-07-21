using System;
using Azure.Core;
using Azure.Identity;
using Microsoft.Azure.Cosmos;

namespace CosmosWithDefaultAzureCreds
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            // Credential class for testing on a local machine or Azure services
            TokenCredential credential = new DefaultAzureCredential();

            // New instance of CosmosClient class using a connection string
            CosmosClient cosmosClient = new(
                accountEndpoint: "https://ch-px-dev-eus-001-cdb.documents.azure.com:443/",
                tokenCredential: credential
            );

            Database cosmodDb = cosmosClient.GetDatabase("px");
            Container cosmosContainer = cosmodDb.GetContainer("tenants");

            cosmosContainer.CreateItemAsync(
                item: new
                {
                    id = "test-item-id",
                    name = "Test Item",
                    description = "This is a test item created using DefaultAzureCredential.",
                    partition = "test-item-partion-key", // 👈 This is required
                },
                partitionKey: new PartitionKey("test-item-partion-key")
            ).GetAwaiter().GetResult();

            cosmosClient.Dispose();
        }
    }
}