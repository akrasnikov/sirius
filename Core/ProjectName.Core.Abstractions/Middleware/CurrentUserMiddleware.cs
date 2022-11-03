using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ProjectName.Core.Abstractions.Interfaces;

namespace ProjectName.Core.Abstractions.Middleware;

public class CurrentUserMiddleware : IMiddleware
{
    private readonly ICurrentUserInitializer _currentUserInitializer;

    public CurrentUserMiddleware(ICurrentUserInitializer currentUserInitializer) =>
        _currentUserInitializer = currentUserInitializer;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        _currentUserInitializer.SetCurrentUser(context.User);

        await next(context);
    }
}