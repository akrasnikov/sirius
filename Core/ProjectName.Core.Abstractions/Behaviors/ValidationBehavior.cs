using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using ProjectName.Core.Abstractions.Wrappers;

namespace ProjectName.Core.Abstractions.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : BaseResponse, new()
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators ?? throw new ArgumentNullException(nameof(validators));
    }
        
    public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var context = new ValidationContext<TRequest>(request);

        var failures = _validators
            .Select(v => v.Validate(context))
            .SelectMany(vr => vr.Errors)
            .Where(vf => vf != null)
            .ToList();

        if (!failures.Any()) return next();
            
        var errors = new Dictionary<string, string>();

        foreach (var failure in failures.Where(failure => !errors.TryAdd(failure.PropertyName, failure.ErrorMessage)))
        {
            if (errors.TryGetValue(failure.PropertyName, out var currentValue))
            {
                errors[failure.PropertyName] = $"{currentValue} / {failure.ErrorMessage}";
            }
        }

        var response = new TResponse
        {
            Status = BaseResponseStatus.Error, 
            Errors = errors
        };
        return Task.FromResult(response);
    }
}