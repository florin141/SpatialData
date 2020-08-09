using System;
using System.Data.Entity;
using System.Diagnostics;
using Spatial.Core;
using Spatial.Core.Data;

namespace Spatial.Data
{
    public abstract class ObjectContextBase : DbContext, IDbContext
    {
        private readonly DatabaseLogWriter _logWriter;

        protected ObjectContextBase(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            _logWriter = new DatabaseLogWriter();
            //Database.Log = _logWriter.Log;
        }

        public new DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }
    }
}
