using ProjectName.Core.Abstractions.Multitenancy;

namespace ProjectName.Core.Abstractions.Interfaces;

public interface ITenantResolver
{
    Tenant GetCurrentTenant();
}