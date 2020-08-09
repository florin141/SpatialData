using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using Spatial.Core;
using Spatial.Core.Data;

namespace Spatial.Data
{
    public class EfRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly IDbContext _context;
        private readonly bool _autoSave;
        private IDbSet<T> _entities;

        public EfRepository(IDbContext context, bool autoSave = true)
        {
            _context = context;
            _autoSave = autoSave;
        }

        public IQueryable<T> Table
        {
            get
            {
                return this.Entities;
            }
        }

        public IQueryable<T> TableUntracked
        {
            get
            {
                return this.Entities.AsNoTracking();
            }
        }

        public T GetById(object id)
        {
            return this.Entities.Find(id);
        }

        public void Insert(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            this.Entities.Add(entity);

            if (_autoSave)
            {
                SaveChanges();
            }
        }

        public void Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            ChangeStateToModifiedIfApplicable(entity);

            if (_autoSave)
            {
                SaveChanges();
            }
        }

        private void ChangeStateToModifiedIfApplicable(T entity)
        {
            var entry = (_context as ObjectContextBase)?.Entry(entity);

            if (entry != null && entry.State == EntityState.Detached)
            {
                // Entity was detached before or was explicitly constructed.
                // This unfortunately sets all properties to modified.
                entry.State = EntityState.Modified;
            }
            else if (entry != null && entry.State == EntityState.Unchanged)
            {
                // We simply do nothing here, because it is ensured now that DetectChanges()
                // gets implicitly called prior SaveChanges().

                //if (this.AutoCommitEnabledInternal && !ctx.Configuration.AutoDetectChangesEnabled)
                //{
                //	_context.DetectChanges();
                //}
            }
        }

        public void Delete(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            Entities.Attach(entity);
            Entities.Remove(entity);

            if (_autoSave)
            {
                SaveChanges();
            }
        }

        public int Delete(string id)
        {
            string query = $"DELETE FROM [dbo].[{TablesName.DesignatedPoint}] WHERE [Id]=@Id";
            if (_context is ObjectContextBase ctx)
            {
                return ctx.Database.ExecuteSqlCommand(
                    query, 
                    new SqlParameter("@Id", id));
            }

            return 0;
        }

        public int SaveChanges(bool detachEntities = true)
        {
            var changes = _context.SaveChanges();

            if (detachEntities && _context is ObjectContextBase ctx)
            {
                var trackedEntries = ctx.ChangeTracker.Entries()
                    .ToList();

                foreach (var entity in trackedEntries)
                {
                    entity.State = EntityState.Detached;
                }
            }

            return changes;
        }

        private DbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                {
                    _entities = _context.Set<T>();
                }

                return _entities as DbSet<T>;
            }
        }
    }
}
