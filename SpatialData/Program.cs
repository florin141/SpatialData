using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using _int.eurocontrol.cfmu.b2b.adrmessage;
using Spatial.Core;
using Spatial.Core.Data;
using Spatial.Core.Domain;
using Spatial.Data;

namespace SpatialData
{
    class Program
    {
        private const string FilePath = "C:\\aixm\\DesignatedPoint.xml";

        private static readonly DesignatedPointFaker DesignatedPointFaker = new DesignatedPointFaker();

        static void Main(string[] args)
        {
            //Generate X entities
            var designatedPoints = GenerateDesignatedPoints(FilePath);

            SynchronizeDesignatedPoints(designatedPoints);
        }

        private static List<DesignatedPoint> GenerateDesignatedPoints(string filePath, int? count = null)
        {
            Trace.WriteLine($"Generating designated points started at: {DateTime.Now}");
            Console.WriteLine($"Generating designated points started at: {DateTime.Now}");

            var processStopwatch = new Stopwatch();
            processStopwatch.Start();

            #region Slow

            var file = File.Open(filePath, FileMode.Open);

            var serializer = new AixmSerializer<ADRMessageType>();

            #endregion

            var adrMessage = serializer.Deserialize(file);

            var query = adrMessage.HasMember
                .Select(x => x.DesignatedPoint);

            if (count.HasValue)
            {
                query = query.Take(count.Value);
            }

            var designatedPoints = query.ToList();

            DbGeographyFactory factory = new DbGeographyFactory();
            var hashSet = new HashSet<DesignatedPoint>();
            foreach (var point in from element in designatedPoints
                                  let timeSlice = element.TimeSlice
                                  let designatedPointTimeSlice = timeSlice
                                  .Select(x => x.DesignatedPointTimeSlice)
                                  .First()
                                  let locationElement = designatedPointTimeSlice.Location
                                  let pointElement = locationElement.Point
                                  let posElement = pointElement.Pos
                                  let srsName = posElement.SrsName.Split(':')
                                  let points = posElement.Value.Split(' ')
                                  select new DesignatedPoint
                                  {
                                      Id = element.Identifier.Value,
                                      Designator = designatedPointTimeSlice.Designator?.Value.ToUpper(),
                                      //Designator = designatedPointTimeSlice.Designator?.Value.ToLower(),
                                      Type = designatedPointTimeSlice.Type?.Value,
                                      Name = designatedPointTimeSlice.Name1?.Value,
                                      Location = factory.CreatePoint(points[0], points[1])
                                  })
            {
                hashSet.Add(point);
            }

            processStopwatch.Stop();
            Trace.WriteLine($"Generating designated points completed, completion time: {DateTime.Now}, {hashSet.Count} rows generated, elapsed time: {processStopwatch.Elapsed:g}");
            Console.WriteLine($"Generating designated points completed, completion time: {DateTime.Now}, {hashSet.Count} rows generated, elapsed time: {processStopwatch.Elapsed:g}");

            return hashSet.ToList();
        }

        private static List<DesignatedPoint> GenerateDesignatedPoints(int count)
        {
            var hashSet = new HashSet<DesignatedPoint>();

            foreach (var entity in DesignatedPointFaker.Generate(count))
            {
                hashSet.Add(entity);
            }

            return hashSet.ToList();
        }

        private static void SynchronizeDesignatedPoints(List<DesignatedPoint> designatedPoints)
        {
            var context = new EfObjectContext("SpatialData");
            var repository = new EfRepository<DesignatedPoint>(context, false);

            // TODO: Optimize
            #region Slow

            var sourceIds = designatedPoints.Select(x => x.Id).ToList();

            var databaseIds = repository.TableUntracked.Select(x => x.Id).ToList();

            var toDeleteIds = databaseIds.Except(sourceIds).ToList();
            var toInsertIds = sourceIds.Where(x => !databaseIds.Contains(x)).ToList();
            var toUpdateIds = databaseIds.Where(x => sourceIds.Contains(x)).ToList();

            var toDeleteEntities = databaseIds.Where(x => toDeleteIds.Contains(x)).ToList();
            var toInsertEntities = designatedPoints.Where(x => toInsertIds.Contains(x.Id)).ToList();
            var toUpdateEntities = designatedPoints.Where(x => toUpdateIds.Contains(x.Id)).ToList();

            #endregion

            Delete(toDeleteEntities, repository);
            Insert(toInsertEntities, repository);
            Update(toUpdateEntities, repository);
        }

        private static void Insert(List<DesignatedPoint> list, IRepository<DesignatedPoint> repository)
        {
            var remainingPoints = list.Count;
            var numberOfPoints = list.Count;

            Trace.WriteLine($"Insert process started at: {DateTime.Now}, {numberOfPoints} entities to insert");
            Console.WriteLine($"Insert process started at: {DateTime.Now}, {numberOfPoints} entities to insert");

            var processStopwatch = new Stopwatch();
            processStopwatch.Start();
            var changes = 0;

            if (!list.Any())
            {
                processStopwatch.Stop();
                Trace.WriteLine($"Insert process completion time: {DateTime.Now}, {changes} rows affected, elapsed time: {processStopwatch.Elapsed:g}");
                Console.WriteLine($"Insert process completion time: {DateTime.Now}, {changes} rows affected, elapsed time: {processStopwatch.Elapsed:g}");
                return;
            }

            var operationStopwatch = new Stopwatch();
            for (var i = 0; i < numberOfPoints; i++)
            {
                var point = list[i];

                operationStopwatch.Start();

                repository.Insert(point);

                Trace.WriteLine($"\tRecord {numberOfPoints - --remainingPoints}/{numberOfPoints} inserted, elapsed time: {operationStopwatch.Elapsed.TotalMilliseconds} ms");

                if (i != 0 && i % 100 == 0 || i == numberOfPoints - 1)
                {
                    changes += repository.SaveChanges();
                }

                operationStopwatch.Reset();
            }

            processStopwatch.Stop();
            Trace.WriteLine($"Insert process completion time: {DateTime.Now}, {changes} rows affected, elapsed time: {processStopwatch.Elapsed:g}");
            Console.WriteLine($"Insert process completion time: {DateTime.Now}, {changes} rows affected, elapsed time: {processStopwatch.Elapsed:g}");
        }

        private static void Update(List<DesignatedPoint> list, IRepository<DesignatedPoint> repository)
        {
            var remainingPoints = list.Count;
            var numberOfPoints = list.Count;

            Trace.WriteLine($"Update process started at: {DateTime.Now}, {numberOfPoints} entities to update");
            Console.WriteLine($"Update process started at: {DateTime.Now}, {numberOfPoints} entities to update");

            var processStopwatch = new Stopwatch();
            processStopwatch.Start();
            var changes = 0;

            if (!list.Any())
            {
                processStopwatch.Stop();
                Trace.WriteLine($"Update process completion time: {DateTime.Now}, {changes} rows affected, elapsed time: {processStopwatch.Elapsed:g}");
                Console.WriteLine($"Update process completion time: {DateTime.Now}, {changes} rows affected, elapsed time: {processStopwatch.Elapsed:g}");
                return;
            }

            var operationStopwatch = new Stopwatch();
            for (var i = 0; i < numberOfPoints; i++)
            {
                var entity = list[i];

                operationStopwatch.Start();

                var fromStore = repository.GetById(entity.Id);

                if (fromStore != entity)
                {
                    fromStore.Name = entity.Name;
                    fromStore.Type = entity.Type;
                    fromStore.Designator = entity.Designator;
                    fromStore.Location = entity.Location;

                    repository.Update(fromStore);

                    Trace.WriteLine($"\tRecord {numberOfPoints - --remainingPoints}/{numberOfPoints} updated, elapsed time: {operationStopwatch.Elapsed.TotalMilliseconds} ms");
                }
                else
                {
                    Trace.WriteLine($"\tRecord {numberOfPoints - --remainingPoints}/{numberOfPoints} identical, elapsed time: {operationStopwatch.Elapsed.TotalMilliseconds} ms");
                }

                if (i != 0 && i % 100 == 0 || i == numberOfPoints - 1)
                {
                    changes += repository.SaveChanges();
                }

                operationStopwatch.Reset();
            }

            processStopwatch.Stop();
            Trace.WriteLine($"Update process completion time: {DateTime.Now}, {changes} rows affected, elapsed time: {processStopwatch.Elapsed:g}");
            Console.WriteLine($"Update process completion time: {DateTime.Now}, {changes} rows affected, elapsed time: {processStopwatch.Elapsed:g}");
        }

        private static void Delete(List<string> list, IRepository<DesignatedPoint> repository)
        {
            var remainingPoints = list.Count;
            var numberOfPoints = list.Count;

            Trace.WriteLine($"Delete process started at: {DateTime.Now}, {numberOfPoints} entities to delete");
            Console.WriteLine($"Delete process started at: {DateTime.Now}, {numberOfPoints} entities to delete");

            var processStopwatch = new Stopwatch();
            processStopwatch.Start();
            var changes = 0;

            if (!list.Any())
            {
                processStopwatch.Stop();
                Trace.WriteLine($"Delete process completion time: {DateTime.Now}, {changes} rows affected, elapsed time: {processStopwatch.Elapsed:g}");
                Console.WriteLine($"Delete process completion time: {DateTime.Now}, {changes} rows affected, elapsed time: {processStopwatch.Elapsed:g}");
                return;
            }

            var operationStopwatch = new Stopwatch();
            for (var i = 0; i < numberOfPoints; i++)
            {
                var id = list[i];

                operationStopwatch.Start();

                changes += repository.Delete(id);

                Trace.WriteLine($"\tRecord {numberOfPoints - --remainingPoints}/{numberOfPoints} deleted, elapsed time: {operationStopwatch.Elapsed.TotalMilliseconds} ms");

                //if (i != 0 && i % 100 == 0 || i == numberOfPoints - 1)
                //{
                //    changes += repository.SaveChanges();
                //}

                operationStopwatch.Reset();
            }

            processStopwatch.Stop();
            Trace.WriteLine($"Delete process completion time: {DateTime.Now}, {changes} rows affected, elapsed time: {processStopwatch.Elapsed:g}");
            Console.WriteLine($"Delete process completion time: {DateTime.Now}, {changes} rows affected, elapsed time: {processStopwatch.Elapsed:g}");
        }
    }
}
