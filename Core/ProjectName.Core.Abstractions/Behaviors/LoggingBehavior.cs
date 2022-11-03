using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ProjectName.Core.Abstractions.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        => _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
        RequestHandlerDelegate<TResponse> next)
    {
        // var serializerOptions = new JsonSerializerOptions
        // {
        //     DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        //     Converters = { new JsonStringEnumConverter() }
        // };
        _logger.LogInformation($"Handling {typeof(TRequest).FullName}");
        // var requestBody = JsonSerializer.Serialize(request, serializerOptions);
        // _logger.LogInformation($"Request body: {requestBody}");
            
        var response = await next();
   
        _logger.LogInformation($@"Handled {typeof(TResponse).FullName}");
        // var responseBody = JsonSerializer.Serialize(response, serializerOptions);
        // _logger.LogInformation($"Response body: {responseBody}");
            
        return response;
    }
}