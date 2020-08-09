using System.Collections.Generic;
using Newtonsoft.Json;

namespace Spatial.Core.Infrastructure
{
    public class FeatureCollection<T> where T : BaseEntity
    {
        public FeatureCollection()
        {
            Features = new List<T>();
        }

        [JsonProperty(PropertyName = "type", Required = Required.Always, DefaultValueHandling = DefaultValueHandling.Include)]
        public string Type => "FeatureCollection";

        [JsonProperty(PropertyName = "features", Required = Required.Always)]
        public List<T> Features { get; private set; }
    }
}