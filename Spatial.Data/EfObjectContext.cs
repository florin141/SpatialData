using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Spatial.Data.Mapping;

namespace Spatial.Data
{
    public class EfObjectContext : ObjectContextBase
    {
        public EfObjectContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<EfObjectContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new DesignatedPointMap());

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
