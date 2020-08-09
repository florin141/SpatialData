using Bogus;
using Spatial.Core.Domain;

namespace SpatialData
{
    public sealed class DesignatedPointFaker : Faker<DesignatedPoint>
    {
        private readonly string[] _types = { "ICAO", "COORD", "CNF", "DESIGNED", "MTR", "TERMINAL", "BRG_DIST", "OTHER" };

        private readonly DbGeographyFactory _factory = new DbGeographyFactory();

        public DesignatedPointFaker()
        {
            RuleFor(d => d.Id, f => f.Random.Guid().ToString());
            RuleFor(d => d.Designator, f => f.Random.String(5, 5, 'A', 'Z'));
            RuleFor(d => d.Type, f => f.PickRandom(_types));
            RuleFor(d => d.Name, 
                f => f.Random.String(3, 3, 'A', 'Z') + 
                     f.Random.String(5, 5, '0', '9'));
            RuleFor(d => d.Location, 
                f => _factory.CreatePoint(f.Random.Int(-90, 90), f.Random.Int(-180, 180)));
        }
    }
}
