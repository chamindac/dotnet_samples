using Azure.Identity;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;

namespace GenerateBlobSaSUri;

class Program
{
    static async Task Main(string[] args)
    {
        BlobServiceClient blobServiceClient = new(
    new Uri("https://cheuw001assetssthot.blob.core.windows.net/"),
    new DefaultAzureCredential());

        BlobContainerClient blobContainer = blobServiceClient.GetBlobContainerClient("dotnet-1e049c87-ce56-4c54-afc8-0c5a01a97bf3");
        BlobClient blob = blobContainer.GetBlobClient("boatnewyork_005.png");
        DateTimeOffset startsOn = DateTimeOffset.UtcNow;
        DateTimeOffset expiresOn = startsOn.AddHours(1);

        UserDelegationKey userDelegationKey = await blobServiceClient.GetUserDelegationKeyAsync(
            startsOn, expiresOn);

        BlobSasBuilder sasBuilder = new()
        {
            BlobContainerName = blob.BlobContainerName,
            BlobName = blob.Name,
            Resource = "b",
            StartsOn = startsOn,
            ExpiresOn = expiresOn
        };

        sasBuilder.SetPermissions(BlobSasPermissions.Read);

        BlobUriBuilder blobUriBuilder = new(blob.Uri)
        {
            Sas = sasBuilder.ToSasQueryParameters(
                    userDelegationKey,
                    blobServiceClient.AccountName)
        };

        Uri uri = blobUriBuilder.ToUri();

        Console.WriteLine(uri.AbsoluteUri);
    }
}
