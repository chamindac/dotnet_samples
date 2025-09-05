using di_sample.domain.core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace di_sample.domain.core.Interfaces
{
    public  interface IOrganizationOperationService
    {
        Task<Organization> CreateOrganizationAsync(string name);

        Task<Organization?> GetOrganizationByNameAsync(string name);

        Task<IEnumerable<Organization>> GetAllAsync();
    }
}
