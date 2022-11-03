using Microsoft.AspNetCore.Identity;
using ProjectName.Auth.Domain.Common.Interfaces;

namespace ProjectName.Auth.Domain.Entities.Identity
{
    public class RoleClaim : IdentityRoleClaim<Guid>, IEntityBase
    {
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string CreatedBy { get; set; } = default!;
        public string ModifiedBy { get; set; } = default!;
        public bool IsDeleted { get; set; } = default;
    }
}