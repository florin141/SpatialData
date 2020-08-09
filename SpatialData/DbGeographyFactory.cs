using System;
using System.Data.Entity.Spatial;
using System.Globalization;

namespace SpatialData
{
    public class DbGeographyFactory
    {
        private const int CoordinateSystemId = 4326;

        public DbGeography CreatePoint(double latitude, double longitude, int coordinateSystemId = CoordinateSystemId)
        {
            if (-90 < latitude && latitude > 90)
            {
                throw new ArgumentOutOfRangeException(nameof(latitude), "Latitudes range from -90 to 90.");
            }

            if (-180 < longitude && longitude > 180)
            {
                throw new ArgumentOutOfRangeException(nameof(longitude), "Longitudes range from -180 to 180.");
            }

            string wkt = string.Format(CultureInfo.InvariantCulture, "POINT({0} {1})", longitude, latitude);

            return DbGeography.PointFromText(wkt, coordinateSystemId);
        }

        public DbGeography CreatePoint(string latitude, string longitude, int coordinateSystemId = CoordinateSystemId)
        {
            if (string.IsNullOrEmpty(latitude))
            {
                throw new ArgumentOutOfRangeException(nameof(latitude), "May not be empty.");
            }

            if (string.IsNullOrEmpty(longitude))
            {
                throw new ArgumentOutOfRangeException(nameof(longitude), "May not be empty.");
            }

            if (!double.TryParse(latitude, NumberStyles.Float, CultureInfo.InvariantCulture, out double lat))
            {
                throw new ArgumentOutOfRangeException(nameof(latitude), "Latitude representation must be a numeric.");
            }

            if (!double.TryParse(longitude, NumberStyles.Float, CultureInfo.InvariantCulture, out double lon))
            {
                throw new ArgumentOutOfRangeException(nameof(longitude), "Longitude representation must be a numeric.");
            }

            return CreatePoint(lat, lon, coordinateSystemId);
        }
    }
}
