using Microsoft.AspNetCore.Identity;
using ProjectName.Auth.Domain.Common.Interfaces;

namespace ProjectName.Auth.Domain.Entities.Identity
{
    public class UserToken : IdentityUserToken<Guid>, IEntityBase
    {
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }       
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}