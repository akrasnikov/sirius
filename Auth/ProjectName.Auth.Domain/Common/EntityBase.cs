using ProjectName.Auth.Domain.Common.Interfaces;

namespace ProjectName.Auth.Domain.Common
{
    public abstract class EntityBase : IEntityBase
    {
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}