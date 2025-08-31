using System.Threading.Tasks;

namespace di_sample.domain.core.Interfaces
{
    public interface IGenericDbRepository<TDomainModel>
    {
        Task<TDomainModel> CreateAsync(TDomainModel organization);
    }
}
