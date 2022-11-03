using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using ProjectName.ServiceName.Application.Exceptions;
using ProjectName.ServiceName.Application.Interfaces.Repositories;
using ProjectName.ServiceName.Application.Wrappers;
using ProjectName.ServiceName.Domain.Entities;

namespace ProjectName.ServiceName.Application.Features.Positions.Queries.GetPositionById
{
    public class GetPositionByIdQuery : IRequest<Response<Position>>
    {
        public Guid Id { get; init; }

        public class GetPositionByIdQueryHandler : IRequestHandler<GetPositionByIdQuery, Response<Position>>
        {
            private readonly IPositionRepositoryAsync _positionRepository;

            public GetPositionByIdQueryHandler(IPositionRepositoryAsync positionRepository)
            {
                _positionRepository = positionRepository;
            }

            public async Task<Response<Position>> Handle(GetPositionByIdQuery query, CancellationToken cancellationToken)
            {
                var position = await _positionRepository.GetByIdAsync(query.Id);
                if (position == null) throw new ApiException($"Position Not Found.");
                return new Response<Position>(position);
            }
        }
    }
}