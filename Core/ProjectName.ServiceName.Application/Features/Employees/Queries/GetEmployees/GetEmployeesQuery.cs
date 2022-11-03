using AutoMapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ProjectName.Core.Abstractions.Entities;
using ProjectName.ServiceName.Application.Interfaces;
using ProjectName.ServiceName.Application.Interfaces.Repositories;
using ProjectName.ServiceName.Application.Parameters;
using ProjectName.ServiceName.Application.Wrappers;

namespace ProjectName.ServiceName.Application.Features.Employees.Queries.GetEmployees
{
    /// <summary>
    /// GetAllEmployeesQuery - handles media IRequest
    /// BaseRequestParameter - contains paging parameters
    /// To add filter/search parameters, add search properties to the body of this class
    /// </summary>
    public class GetEmployeesQuery : QueryParameter, IRequest<PagedResponse<IEnumerable<SerializableEntity>>>
    {
        //examples:
        public string EmployeeNumber { get; set; }

        public string EmployeeTitle { get; set; }
    }

    public class GetAllEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, PagedResponse<IEnumerable<SerializableEntity>>>
    {
        private readonly IEmployeeRepositoryAsync _employeeRepository;
        private readonly IMapper _mapper;
        private readonly IModelHelper _modelHelper;

        public GetAllEmployeesQueryHandler(IEmployeeRepositoryAsync employeeRepository, IMapper mapper, IModelHelper modelHelper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _modelHelper = modelHelper;
        }

        public async Task<PagedResponse<IEnumerable<SerializableEntity>>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
        {
            var validFilter = request;
            //filtered fields security
            if (!string.IsNullOrEmpty(validFilter.Fields))
            {
                //limit to fields in view model
                validFilter.Fields = _modelHelper.ValidateModelFields<GetEmployeesViewModel>(validFilter.Fields);
            }
            if (string.IsNullOrEmpty(validFilter.Fields))
            {
                //default fields from view model
                validFilter.Fields = _modelHelper.GetModelFields<GetEmployeesViewModel>();
            }
            // query based on filter
            var entityEmployees = await _employeeRepository.GetPagedEmployeeReponseAsync(validFilter);
            var data = entityEmployees.data;
            RecordsCount recordCount = entityEmployees.recordsCount;

            // response wrapper
            return new PagedResponse<IEnumerable<SerializableEntity>>(data, validFilter.PageNumber, validFilter.PageSize, recordCount);
        }
    }
}