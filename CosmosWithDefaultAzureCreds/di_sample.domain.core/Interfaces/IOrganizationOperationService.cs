using di_sample.domain.core.Models;
using System.Threading.Tasks;

namespace di_sample.domain.core.Interfaces
{
    public  interface IOrganizationOperationService
    {
        Task<Organization> CreateOrganizationAsync(string name);
    }
}
