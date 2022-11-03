using System;
using ProjectName.Core.Abstractions.Interfaces;

namespace ProjectName.Core.Abstractions.Entities;

public abstract class BaseEntity : BaseEntity<Guid>
{
    protected BaseEntity()
    {
        Id = Guid.NewGuid();
    }
}

public abstract class BaseEntity<TId> : IEntity<TId>
{
    public TId Id { get; set; } = default!;
}

public abstract class AuditableBaseEntity : BaseEntity
{
    public string CreatedBy { get; set; }
    public DateTime Created { get; set; }
    public string LastModifiedBy { get; set; }
    public DateTime? LastModified { get; set; }
}