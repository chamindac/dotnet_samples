using di_sample.domain.core.Models;
using System.Threading;
using System.Threading.Tasks;

namespace di_sample.domain.core.Interfaces
{
    public interface IOrganizationDbRepository<TDomainModel>
    {
        Task<TDomainModel> CreateAsync(TDomainModel organization);

        Task<Organization?> GetOrganizationByNameAsync(
            string name,
            CancellationToken cancellationToken = default);
    }
}
