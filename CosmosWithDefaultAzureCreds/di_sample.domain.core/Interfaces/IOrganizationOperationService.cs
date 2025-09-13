using di_sample.domain.infrastrcture.Models.Db;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace di_sample.domain.core.Interfaces
{
    public  interface IOrganizationOperationService
    {
        Task<OrganizationDbModel> CreateOrganizationAsync(string name);

        Task<OrganizationDbModel?> GetOrganizationByNameAsync(string name);

        Task<IEnumerable<OrganizationDbModel>> GetAllAsync();
    }
}
