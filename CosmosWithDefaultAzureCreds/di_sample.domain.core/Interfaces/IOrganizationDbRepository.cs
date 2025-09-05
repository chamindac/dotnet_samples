using di_sample.domain.core.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace di_sample.domain.core.Interfaces
{
    public interface IOrganizationDbRepository<TDomainModel>
    {
        Task<TDomainModel> CreateAsync(TDomainModel organization);

        Task<Organization?> GetByNameAsync(string name);

        Task<IEnumerable<Organization>> GetAllAsync();
    }
}
