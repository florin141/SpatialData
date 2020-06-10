using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace SpatialData
{
    public class SpatialContext : DbContext
    {
        public DbSet<University> Universities { get; set; }

        public SpatialContext() 
            : base("SpatialData")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
