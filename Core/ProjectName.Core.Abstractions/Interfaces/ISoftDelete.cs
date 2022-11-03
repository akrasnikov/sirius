using System;

namespace ProjectName.Core.Abstractions.Interfaces
{
    public interface ISoftDelete
    {
        public DateTime? DeletedOn { get; set; }
        public Guid? DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
