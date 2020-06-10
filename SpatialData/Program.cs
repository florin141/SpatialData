using System;
using System.Linq;
using NetTopologySuite.Geometries;

namespace SpatialData
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new SpatialContext())
            {
                context.Universities.Add(new University()
                {
                    Name = "Graphic Design Institute",
                    Location = new Point(-122.336106, 47.605049)
                    {
                        SRID = 4326
                    },
                });

                context.Universities.Add(new University()
                {
                    Name = "School of Fine Art",
                    Location = new Point(-122.335197, 47.646711)
                    {
                        SRID = 4326
                    },
                });

                context.SaveChanges();

                var myLocation = new Point(-122.296623, 47.640405)
                {
                    SRID = 4326
                };

                var university = (from u in context.Universities
                    orderby u.Location.Distance(myLocation)
                    select u).FirstOrDefault();

                Console.WriteLine(
                    "The closest University to you is: {0}.", university.Name);
            }
        }
    }
}
