using System.Data.Entity.Spatial;

namespace SpatialData
{
    public class University
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DbGeography Location { get; set; }
    }
}
