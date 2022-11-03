using System;

namespace ProjectName.Core.Abstractions.Interfaces;

public interface IAuditableEntity
{
    public Guid CreatedBy { get; set; }
    public DateTime CreatedOn { get; }
    public Guid LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
}