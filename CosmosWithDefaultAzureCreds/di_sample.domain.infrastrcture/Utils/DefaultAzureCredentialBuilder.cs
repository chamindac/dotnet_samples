using Azure.Identity;

namespace di_sample.domain.infrastrcture.Utils
{
    internal static class DefaultAzureCredentialBuilder
    {
        internal static DefaultAzureCredential Build(string tenantId)
        {
            return new DefaultAzureCredential(
                new DefaultAzureCredentialOptions
                {
                    TenantId = tenantId
                });
        }
    }
}
