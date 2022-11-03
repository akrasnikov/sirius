using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectName.Core.Abstractions.Entities;

namespace ProjectName.ServiceName.Application.Interfaces
{
    public interface IDataShapeHelper<T>
    {
        IEnumerable<SerializableEntity> ShapeData(IEnumerable<T> entities, string fieldsString);

        Task<IEnumerable<SerializableEntity>> ShapeDataAsync(IEnumerable<T> entities, string fieldsString);

        SerializableEntity ShapeData(T entity, string fieldsString);
    }
}