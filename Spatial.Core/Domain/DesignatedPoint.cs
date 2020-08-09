using System.Data.Entity.Spatial;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Spatial.Core.Infrastructure;

namespace Spatial.Core.Domain
{
    /// <summary>
    /// A geographical location not marked by the site of a radio navigation aid, used in defining an ATS route,
    /// the flight path of an aircraft or for other navigation or ATS purposes. 
    /// </summary>
    [JsonConverter(typeof(FeatureConverter<DesignatedPoint>))]
    public class DesignatedPoint : BaseEntity
    {
        /// <summary>
        /// The coded designator of the point. For example, the five-letter ICAO name of the point, etc.. 
        /// </summary>
        public string Designator { get; set; }

        /// <summary>
        /// The kind of point designator, indicating the rules by which the designator has been created.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// The full textual name of a designated point, if any. For example, 'GOTAN Intersection' for GOTAN.
        /// Name may also be used to identify an unnamed point (See ARINC 424 for unnamed points. 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The geographical location of the designated point.
        /// </summary>
        // [JsonProperty(PropertyName = "geometry", Required = Required.Always)]
        [JsonConverter(typeof(DbGeographyConverter))]
        public DbGeography Location { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as DesignatedPoint;

            if (other == null)
            {
                return base.Equals(obj);
            }

            return Id == other.Id && 
                   Designator == other.Designator &&
                   Type == other.Type &&
                   Name == other.Name &&
                   Location.SpatialEquals(other.Location);
        }

        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ (Designator != null ? Designator.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Type != null ? Type.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Location != null ? Location.ToString().GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
