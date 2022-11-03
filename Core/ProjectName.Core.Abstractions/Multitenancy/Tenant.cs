namespace ProjectName.Core.Abstractions.Multitenancy;

public class Tenant
{
    public string Name { get; set; } = null!;
    public string Secret { get; set; } = null!;
    public string? ConnectionString { get; set; }
}