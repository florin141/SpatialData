using System.Collections.Generic;
using GeoJSON.Net.Geometry;

namespace SpatialData
{
    public static class SampleGeoJsonTypes
    {
        private static Point _point;
        private static MultiPoint _multiPoint;
        private static LineString _lineString;
        private static MultiLineString _multiLineString;
        private static Polygon _polygon;
        private static MultiPolygon _multiPolygon;
        private static GeometryCollection _geomCollection;

        public static Point Point
        {
            get
            {
                if (_point == null)
                {
                    _point = new Point(new Position(53.2455662, 90.65464646));
                }

                return _point;
            }
        }

        public static MultiPoint MultiPoint
        {
            get
            {
                if (_multiPoint == null)
                {
                    _multiPoint = new MultiPoint(new List<Point>
                    {
                        new Point(new Position(52.379790828551016, 5.3173828125)),
                        new Point(new Position(52.36721467920585, 5.456085205078125)),
                        new Point(new Position(52.303440474272755, 5.386047363281249, 4.23))
                    });
                }

                return _multiPoint;
            }
        }

        public static LineString LineString
        {
            get
            {
                if (_lineString == null)
                {
                    _lineString = new LineString(new List<IPosition>
                    {
                        new Position(52.379790828551016, 5.3173828125),
                        new Position(52.36721467920585, 5.456085205078125),
                        new Position(52.303440474272755, 5.386047363281249, 4.23)
                    });
                }

                return _lineString;
            }
        }

        public static MultiLineString MultiLineString
        {
            get
            {
                if (_multiLineString == null)
                {
                    _multiLineString = new MultiLineString(new List<LineString>
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
                }

                return _multiLineString;
            }
        }

        public static Polygon Polygon
        {
            get
            {
                if (_polygon == null)
                {
                    _polygon = new Polygon(new List<LineString>
                    {
                        new LineString(new List<Position>
                        {
                            new Position(52.379790828551016, 5.3173828125),
                            new Position(52.303440474272755, 5.386047363281249, 4.23),
                            new Position(52.36721467920585, 5.456085205078125),
                            new Position(52.379790828551016, 5.3173828125)
                        })
                    });
                }

                return _polygon;
            }
        }

        public static MultiPolygon MultiPolygon
        {
            get
            {
                if (_multiPolygon == null)
                {
                    _multiPolygon = new MultiPolygon(new List<Polygon>
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
                }

                return _multiPolygon;
            }
        }

        public static GeometryCollection GeometryCollection
        {
            get
            {
                if (_geomCollection == null)
                {
                    _geomCollection = new GeometryCollection(new List<IGeometryObject>
                    {
                        Point,
                        MultiPoint,
                        LineString,
                        MultiLineString,
                        Polygon,
                        MultiPolygon
                    });
                }

                return _geomCollection;
            }
        }

    }
}
