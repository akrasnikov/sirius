using System;

namespace ProjectName.Core.Abstractions.Interfaces
{
    public interface ITenantable
    {
        public string? TenantId { get; set; }
    }
}
