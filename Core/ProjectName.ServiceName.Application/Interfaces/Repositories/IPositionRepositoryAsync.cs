using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectName.Core.Abstractions.Entities;
using ProjectName.ServiceName.Application.Features.Positions.Queries.GetPositions;
using ProjectName.ServiceName.Application.Parameters;
using ProjectName.ServiceName.Domain.Entities;

namespace ProjectName.ServiceName.Application.Interfaces.Repositories
{
    public interface IPositionRepositoryAsync : IGenericRepositoryAsync<Position>
    {
        Task<bool> IsUniquePositionNumberAsync(string positionNumber);

        Task<bool> SeedDataAsync(int rowCount);

        Task<(IEnumerable<SerializableEntity> data, RecordsCount recordsCount)> GetPagedPositionReponseAsync(GetPositionsQuery requestParameters);
    }
}