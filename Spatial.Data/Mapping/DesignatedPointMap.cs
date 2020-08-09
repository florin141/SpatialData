using System.Data.Entity.ModelConfiguration;
using Spatial.Core.Domain;

namespace Spatial.Data.Mapping
{
    public class DesignatedPointMap : EntityTypeConfiguration<DesignatedPoint>
    {
        public DesignatedPointMap()
        {
            this.ToTable(TablesName.DesignatedPoint);
            this.HasKey(s => s.Id);
        }
    }
}
