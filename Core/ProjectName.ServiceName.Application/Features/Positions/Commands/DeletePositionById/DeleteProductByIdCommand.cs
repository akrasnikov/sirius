using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using ProjectName.ServiceName.Application.Exceptions;
using ProjectName.ServiceName.Application.Interfaces.Repositories;
using ProjectName.ServiceName.Application.Wrappers;

namespace ProjectName.ServiceName.Application.Features.Positions.Commands.DeletePositionById
{
    public class DeletePositionByIdCommand : IRequest<Response<Guid>>
    {
        public Guid Id { get; init; }

        public class DeletePositionByIdCommandHandler : IRequestHandler<DeletePositionByIdCommand, Response<Guid>>
        {
            private readonly IPositionRepositoryAsync _positionRepository;

            public DeletePositionByIdCommandHandler(IPositionRepositoryAsync positionRepository)
            {
                _positionRepository = positionRepository;
            }

            public async Task<Response<Guid>> Handle(DeletePositionByIdCommand command, CancellationToken cancellationToken)
            {
                var position = await _positionRepository.GetByIdAsync(command.Id);
                if (position == null) throw new ApiException($"Position Not Found.");
                await _positionRepository.DeleteAsync(position);
                return new Response<Guid>(position.Id);
            }
        }
    }
}