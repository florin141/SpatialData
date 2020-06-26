using System.Data.Entity.Spatial;

namespace SpatialData.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DbGeography Geography { get; set; }
    }
}
