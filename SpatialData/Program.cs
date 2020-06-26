using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using GeoJSON.Net.Contrib.EntityFramework;
using GeoJSON.Net.CoordinateReferenceSystem;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using SpatialData.Converters;
using SpatialData.Entities;

namespace SpatialData
{
    class Program
    {
        private static Point point;
        private static MultiPoint multiPoint;
        private static LineString lineString;
        private static MultiLineString multiLineString;
        private static Polygon polygon;
        private static MultiPolygon multiPolygon;
        private static GeometryCollection geomCollection;

        private static JsonConverter dbGeographyConverter = new DbGeographyConverter();

        static void Main(string[] args)
        {
            Initialize();

            using (var context = new SpatialContext())
            {
                var entityToSave = new GeometryCollectionEntity()
                {
                    Name = nameof(GeometryCollectionEntity),
                    Geography = geomCollection.ToDbGeography()
                };

                context.GeometryCollection.Add(entityToSave);

                context.SaveChanges();

                var pointFromDb = context.GeometryCollection
                    .FirstOrDefault();

                if (pointFromDb != null)
                {
                    var pointFromGeoJson = JsonConvert.SerializeObject(pointFromDb, 
                        Formatting.Indented,
                        dbGeographyConverter);

                    var entityFromGeoJson = JsonConvert.DeserializeObject<PointEntity>(pointFromGeoJson, 
                        dbGeographyConverter);

                    var entityAsStr = JsonConvert.SerializeObject(entityFromGeoJson,
                        Formatting.Indented,
                        dbGeographyConverter);

                    if (pointFromGeoJson == entityAsStr)
                    {
                        Console.WriteLine("Equal");
                    }
                }
            }
        }

        private static void Initialize()
        {
            point = new Point(new Position(53.2455662, 90.65464646));

            multiPoint = new MultiPoint(new List<Point>
            {
                new Point(new Position(52.379790828551016, 5.3173828125)),
                new Point(new Position(52.36721467920585, 5.456085205078125)),
                new Point(new Position(52.303440474272755, 5.386047363281249, 4.23))
            });
            lineString = new LineString(new List<IPosition>
            {
                new Position(52.379790828551016, 5.3173828125),
                new Position(52.36721467920585, 5.456085205078125),
                new Position(52.303440474272755, 5.386047363281249, 4.23)
            });
            multiLineString = new MultiLineString(new List<LineString>
            {
                new LineString(new List<IPosition>
                {
                    new Position(52.379790828551016, 5.3173828125),
                    new Position(52.36721467920585, 5.456085205078125),
                    new Position(52.303440474272755, 5.386047363281249, 4.23)
                }),
                new LineString(new List<IPosition>
                {
                    new Position(52.379790828551016, 5.3273828125),
                    new Position(52.36721467920585, 5.486085205078125),
                    new Position(52.303440474272755, 5.426047363281249, 4.23)
                })
            });
            polygon = new Polygon(new List<LineString>
                {
                    new LineString(new List<Position>
                    {
                        new Position(52.379790828551016, 5.3173828125),
                        new Position(52.303440474272755, 5.386047363281249, 4.23),
                        new Position(52.36721467920585, 5.456085205078125),
                        new Position(52.379790828551016, 5.3173828125)
                    })
                });
            multiPolygon = new MultiPolygon(new List<Polygon>
                {
                    new Polygon(new List<LineString>
                    {
                        new LineString(new List<IPosition>
                        {
                            new Position(52.959676831105995, -2.6797102391514338),
                            new Position(52.930592009390175, -2.6548779332193022),
                            new Position(52.89564268523565, -2.6931334629377890),
                            new Position(52.878791122091066, -2.6932445076063951),
                            new Position(52.875255907042678, -2.6373482332006359),
                            new Position(52.882954723868622, -2.6050779098387191),
                            new Position(52.875476700983896, -2.5851645010668989),
                            new Position(52.891287242948195, -2.5815104708998668),
                            new Position(52.908449372833715, -2.6079763270327119),
                            new Position(52.9608756693609, -2.6769029474483279),
                            new Position(52.959676831105995, -2.6797102391514338),
                        })
                    }),
                    new Polygon(new List<LineString>
                    {
                        new LineString(new List<IPosition>
                        {
                            new Position(52.89610842810761, -2.69628632041613),
                            new Position(52.926572918779222, -2.6996509024137052),
                            new Position(52.920394929466184, -2.772273870352612),
                            new Position(52.937353122653533, -2.7978187468478741),
                            new Position(52.94013913205788, -2.838979264607087),
                            new Position(52.929801009654575, -2.83848602260174),
                            new Position(52.90253773227807, -2.804554822840895),
                            new Position(52.89938894657412, -2.7663172788742449),
                            new Position(52.8894641454077, -2.75901233808515),
                            new Position(52.89610842810761, -2.69628632041613)
                        })
                    })
                });
            geomCollection = new GeometryCollection(new List<IGeometryObject>
            {
                point,
                multiPoint,
                lineString,
                multiLineString,
                polygon,
                multiPolygon
            });
        }
    }
}
