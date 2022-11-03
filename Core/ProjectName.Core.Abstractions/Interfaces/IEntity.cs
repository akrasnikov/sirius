namespace ProjectName.Core.Abstractions.Interfaces;

public interface IEntity
{
}

public interface IEntity<TId> : IEntity
{
    TId Id { get; set; }
}