using di_sample.domain.core.Models;
using di_sample.domain.infrastrcture.Models.Db;
using Riok.Mapperly.Abstractions;

namespace di_sample.domain.infrastrcture.Mappers
{
    [Mapper]
    internal static partial class OrganizationMapper
    {
        // Core -> DB
        public static partial OrganizationCosmosDbModel ToDbModel(this Organization org);

        // DB -> Core
        public static partial Organization ToDomainModel(this OrganizationCosmosDbModel orgDb);
    }
}
