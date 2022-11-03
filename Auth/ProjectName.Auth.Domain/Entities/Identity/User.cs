using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using ProjectName.Auth.Domain.Common.Interfaces;

namespace ProjectName.Auth.Domain.Entities.Identity
{
    public class User : IdentityUser<Guid>, IEntityBase
    {
        [StringLength(128)] public string FirstName { get; set; } = default!;
        [StringLength(128)] public string LastName { get; set; } = default!;
        [StringLength(128)] public string MiddleName { get; set; } = default!;
        public DateTime? Birthday { get; set; }
        [StringLength(32)] public string Gender { get; set; } = default!;
        [Required] public string Language { get; set; } = default!;

        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
       
        public string? CreatedBy { get ; set ; } = default!;
        public string? ModifiedBy { get ; set ; } = default!;
        public bool IsDeleted { get; set; }

        //public List<Device> Devices { get; set; }       

    }
}