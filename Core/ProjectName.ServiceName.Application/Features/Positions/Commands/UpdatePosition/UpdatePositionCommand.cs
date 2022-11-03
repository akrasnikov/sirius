using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using ProjectName.ServiceName.Application.Exceptions;
using ProjectName.ServiceName.Application.Interfaces.Repositories;
using ProjectName.ServiceName.Application.Wrappers;

namespace ProjectName.ServiceName.Application.Features.Positions.Commands.UpdatePosition
{
    public class UpdatePositionCommand : IRequest<Response<Guid>>
    {
        public Guid Id { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }
        public decimal Salary { get; init; }

        public class UpdatePositionCommandHandler : IRequestHandler<UpdatePositionCommand, Response<Guid>>
        {
            private readonly IPositionRepositoryAsync _positionRepository;

            public UpdatePositionCommandHandler(IPositionRepositoryAsync positionRepository)
            {
                _positionRepository = positionRepository;
            }

            public async Task<Response<Guid>> Handle(UpdatePositionCommand command, CancellationToken cancellationToken)
            {
                var position = await _positionRepository.GetByIdAsync(command.Id);

                if (position == null)
                {
                    throw new ApiException($"Position Not Found.");
                }
                else
                {
                    position.PositionTitle = command.Title;
                    position.PositionSalary = command.Salary;
                    position.PositionDescription = command.Description;
                    await _positionRepository.UpdateAsync(position);
                    return new Response<Guid>(position.Id);
                }
            }
        }
    }
}