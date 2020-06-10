﻿using System;
using System.Data.Entity.Spatial;
using System.Linq;

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
                    Location = DbGeography.FromText("POINT(-122.336106 47.605049)")
                });

                context.Universities.Add(new University()
                {
                    Name = "School of Fine Art",
                    Location = DbGeography.FromText("POINT(-122.335197 47.646711)")
                });

                context.SaveChanges();

                var myLocation = DbGeography.FromText("POINT(-122.296623 47.640405)");

                var university = (from u in context.Universities
                    orderby u.Location.Distance(myLocation)
                    select u).FirstOrDefault();

                if (university != null)
                    Console.WriteLine(
                        "The closest University to you is: {0}.", university.Name);
            }
        }
    }
}
