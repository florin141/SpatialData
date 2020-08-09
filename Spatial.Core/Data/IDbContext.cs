using System.Data.Entity;

namespace Spatial.Core.Data
{
    public interface IDbContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity;

        int SaveChanges();
    }
}
