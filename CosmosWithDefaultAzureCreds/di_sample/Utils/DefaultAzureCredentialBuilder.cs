using Azure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace di_sample.Utils
{
    public static class DefaultAzureCredentialBuilder
    {
        public static DefaultAzureCredential Build(string tenantId)
        {
            return new DefaultAzureCredential(
                new DefaultAzureCredentialOptions
                {
                    TenantId = tenantId
                });
        }
    }
}
