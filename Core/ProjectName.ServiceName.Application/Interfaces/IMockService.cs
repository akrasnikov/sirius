using System.Collections.Generic;
using ProjectName.ServiceName.Domain.Entities;

namespace ProjectName.ServiceName.Application.Interfaces;

public interface IMockService
{
    List<Position> GetPositions(int rowCount);

    IEnumerable<Employee> GetEmployees(int rowCount);

    IEnumerable<Position> SeedPositions(int rowCount);
}