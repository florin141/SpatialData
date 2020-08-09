using System.Linq;

namespace Spatial.Core.Data
{
    public interface IRepository<T> where T : BaseEntity
    {
        IQueryable<T> Table { get; }

        IQueryable<T> TableUntracked { get; }

        T GetById(object id);

        void Insert(T entity);

        void Update(T entity);

        void Delete(T entity);

        int Delete(string id);

        int SaveChanges(bool detachEntities = true);
    }
}
