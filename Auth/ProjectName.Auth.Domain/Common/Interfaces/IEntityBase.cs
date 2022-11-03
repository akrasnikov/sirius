namespace ProjectName.Auth.Domain.Common.Interfaces
{
    public interface IEntityBase
    {
        DateTime CreatedDate { get; set; }
        DateTime ModifiedDate { get; set; }
        string CreatedBy { get; set; }
        string ModifiedBy { get; set; }
        bool IsDeleted { get; set; }
    }
}