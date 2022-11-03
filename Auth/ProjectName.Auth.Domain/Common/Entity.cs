using ProjectName.Auth.Domain.Common.Interfaces;

namespace ProjectName.Auth.Domain.Common
{
    //Auditable
    public abstract class Entity : EntityBase, IEntity
    {

    }

    public abstract class Entity<TKey> : EntityBase, IEntity<TKey>
    {
        public TKey Id { get; set; }
    }
}