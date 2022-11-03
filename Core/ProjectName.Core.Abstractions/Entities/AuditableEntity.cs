using System;
using ProjectName.Core.Abstractions.Interfaces;

namespace ProjectName.Core.Abstractions.Entities;

public abstract class AuditableEntity : AuditableEntity<Guid>
{
}


public abstract class AuditableEntity<T> : BaseEntity<T>, IAuditableEntity, ISoftDelete, ITenantable
{
    public Guid CreatedBy { get; set; }
    public DateTime CreatedOn { get; private set; }
    public Guid LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public DateTime? DeletedOn { get; set; }
    public Guid? DeletedBy { get; set; }
    public bool IsDeleted { get; set; }
    public string? TenantId { get; set; }

    protected AuditableEntity()
    {
        CreatedOn = DateTime.UtcNow;
        LastModifiedOn = DateTime.UtcNow;
    }
}