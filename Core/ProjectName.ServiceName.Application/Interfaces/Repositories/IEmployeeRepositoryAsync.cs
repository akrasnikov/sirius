using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectName.Core.Abstractions.Entities;
using ProjectName.ServiceName.Application.Features.Employees.Queries.GetEmployees;
using ProjectName.ServiceName.Application.Parameters;
using ProjectName.ServiceName.Domain.Entities;

namespace ProjectName.ServiceName.Application.Interfaces.Repositories
{
    public interface IEmployeeRepositoryAsync : IGenericRepositoryAsync<Employee>
    {
        Task<(IEnumerable<SerializableEntity> data, RecordsCount recordsCount)> GetPagedEmployeeReponseAsync(GetEmployeesQuery requestParameter);
    }
}