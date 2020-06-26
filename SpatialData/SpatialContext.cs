using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using SpatialData.Entities;

namespace SpatialData
{
    public class SpatialContext : DbContext
    {
        public DbSet<PointEntity> Points { get; set; }
        public DbSet<MultiPointEntity> MultiPoint { get; set; }
        public DbSet<LineStringEntity> LineString { get; set; }
        public DbSet<MultiLineStringEntity> MultiLineString { get; set; }
        public DbSet<PolygonEntity> Polygon { get; set; }
        public DbSet<MultiPolygonEntity> MultiPolygon { get; set; }
        public DbSet<GeometryCollectionEntity> GeometryCollection { get; set; }
        public DbSet<FeatureEntity> Feature { get; set; }
        public DbSet<FeatureCollectionEntity> FeatureCollection { get; set; }

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
