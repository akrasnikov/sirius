using System.Security.Claims;

namespace ProjectName.Core.Abstractions.Interfaces;

public interface ICurrentUserInitializer
{
    void SetCurrentUser(ClaimsPrincipal user);

    void SetCurrentUserId(string userId);
}