using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Azure.Identity;
using Azure;
using Azure.Storage.Sas;
using System.Reflection.Metadata;

namespace CopyBlobBetweenStorage;

class Program
{
    static async Task Main(string[] args)
    {
        // Source and destination blob storage account connection strings
        Uri sourceStorageUri = new("https://cheuw001assetsstcool.blob.core.windows.net/");
        Uri destinationStorageUri = new("https://cheuw001assetssthot.blob.core.windows.net/");

        // Source blob information
        string sourceContainerName = "originals-de1885b94150-d6f6b9f9-f2eb-42cf-96c5-fe0be098fef3";
        string sourceBlobPath = "1e049c87-ce56-4c54-afc8-0c5a01a97bf3/original";

        // Destination blob information
        string destinationContainerName = "dotnet-copiedfromcool";
        string destinationBlobPath = "1e049c87-ce56-4c54-afc8-0c5a01a97bf3/boat.mp4";

        // Create default azure credentials
        // Make sure the user or managed identity has 
        //  Blob Data Reader permission for source storage
        // Blob Data Contributor for destination storage
        DefaultAzureCredential azureCreds = new();

        // Create BlobServiceClient instances for source and destination accounts
        BlobServiceClient sourceBlobServiceClient = new (sourceStorageUri, azureCreds);
        BlobServiceClient destinationBlobServiceClient = new (destinationStorageUri, azureCreds);

        // Get source blob client
        BlobClient sourceBlobClient = sourceBlobServiceClient
            .GetBlobContainerClient(sourceContainerName)
            .GetBlobClient(sourceBlobPath);

        // Generate SaS url for source blob
        DateTimeOffset startsOn = DateTimeOffset.UtcNow;
        // Depending on the size of the blob to copy, you may need to create a SaS token valid for more than one hour
        DateTimeOffset expiresOn = startsOn.AddHours(1);

        UserDelegationKey userDelegationKey = await sourceBlobServiceClient.GetUserDelegationKeyAsync(
            startsOn, expiresOn);

        BlobSasBuilder sasBuilder = new()
        {
            BlobContainerName = sourceBlobClient.BlobContainerName,            
            BlobName = sourceBlobClient.Name,
            Resource = "b",
            StartsOn = startsOn,
            ExpiresOn = expiresOn
        };

        sasBuilder.SetPermissions(BlobSasPermissions.Read);

        BlobUriBuilder blobUriBuilder = new(sourceBlobClient.Uri)
        {
            Sas = sasBuilder.ToSasQueryParameters(
                    userDelegationKey,
                    sourceBlobServiceClient.AccountName)
        };

        Uri sourceBlobSasuri = blobUriBuilder.ToUri();

        // Get destination blob client
        BlobClient destinationBlobClient = destinationBlobServiceClient
            .GetBlobContainerClient(destinationContainerName)
            .GetBlobClient(destinationBlobPath);

        // Start the copy operation
        CopyFromUriOperation copyOperation = await destinationBlobClient.StartCopyFromUriAsync(sourceBlobSasuri);

        // Check the copy status
        while (copyOperation.GetRawResponse().Status == 202)
        {
            // Optional: Delay to avoid flooding with requests
            await Task.Delay(500);

            // Fetch the latest copy status
            Response<BlobProperties> properties = await destinationBlobClient.GetPropertiesAsync();
            if (properties.Value.CopyStatus == CopyStatus.Pending)
            {
                Console.WriteLine("Copy is still in progress...");
            }
            else if (properties.Value.CopyStatus == CopyStatus.Success)
            {
                Console.WriteLine("Blob copy completed successfully.");
                break;
            }
            else
            {
                Console.WriteLine($"Copy failed with status: {properties.Value.CopyStatus}");
                break;
            }
        }
    }
}
