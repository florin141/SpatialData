using System.Data.Entity.Spatial;
using NetTopologySuite.Geometries;

namespace SpatialData
{
    public class University
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Point Location { get; set; }
    }
}
