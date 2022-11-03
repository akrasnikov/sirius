using FluentValidation;
using System.Threading;
using System.Threading.Tasks;
using ProjectName.ServiceName.Application.Interfaces.Repositories;

namespace ProjectName.ServiceName.Application.Features.Positions.Commands.CreatePosition
{
    public class CreatePositionCommandValidator : AbstractValidator<CreatePositionCommand>
    {
        private readonly IPositionRepositoryAsync _positionRepository;

        public CreatePositionCommandValidator(IPositionRepositoryAsync positionRepository)
        {
            this._positionRepository = positionRepository;

            RuleFor(p => p.PositionNumber)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.")
                .MustAsync(IsUniquePositionNumber).WithMessage("{PropertyName} already exists.");

            RuleFor(p => p.PositionTitle)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");
        }

        private async Task<bool> IsUniquePositionNumber(string positionNumber, CancellationToken cancellationToken)
        {
            return await _positionRepository.IsUniquePositionNumberAsync(positionNumber);
        }
    }
}