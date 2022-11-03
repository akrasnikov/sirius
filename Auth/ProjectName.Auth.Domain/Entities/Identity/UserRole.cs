using System;
using Microsoft.AspNetCore.Identity;

namespace ProjectName.Auth.Domain.Entities.Identity
{
    public class UserRole : IdentityUserRole<Guid>
    {
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}