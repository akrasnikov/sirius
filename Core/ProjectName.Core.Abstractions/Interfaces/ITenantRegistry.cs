using ProjectName.Core.Abstractions.Multitenancy;

namespace ProjectName.Core.Abstractions.Interfaces;

public interface ITenantRegistry
{
    Tenant[] GetTenants();

    // User[] GetUsers();
}