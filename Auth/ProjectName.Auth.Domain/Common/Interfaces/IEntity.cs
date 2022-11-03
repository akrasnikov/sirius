namespace ProjectName.Auth.Domain.Common.Interfaces
{
    public interface IEntity : IEntityBase
    {
    }
    
    public interface IEntity<TKey> : IEntityBase
    {
        TKey Id { get; set; }
    }
}