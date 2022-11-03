using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using ProjectName.ServiceName.Application.Interfaces.Repositories;
using ProjectName.ServiceName.Application.Wrappers;
using ProjectName.ServiceName.Domain.Entities;

namespace ProjectName.ServiceName.Application.Features.Positions.Commands.CreatePosition
{
    public partial class CreatePositionCommand : IRequest<Response<Guid>>
    {
        public string PositionTitle { get; init; }
        public string PositionNumber { get; init; }
        public string PositionDescription { get; init; }
        public decimal PositionSalary { get; init; }
    }

    public class CreatePositionCommandHandler : IRequestHandler<CreatePositionCommand, Response<Guid>>
    {
        private readonly IPositionRepositoryAsync _positionRepository;
        private readonly IMapper _mapper;

        public CreatePositionCommandHandler(IPositionRepositoryAsync positionRepository, IMapper mapper)
        {
            _positionRepository = positionRepository;
            _mapper = mapper;
        }

        public async Task<Response<Guid>> Handle(CreatePositionCommand request, CancellationToken cancellationToken)
        {
            var position = _mapper.Map<Position>(request);
            await _positionRepository.AddAsync(position);
            return new Response<Guid>(position.Id);
        }
    }
}