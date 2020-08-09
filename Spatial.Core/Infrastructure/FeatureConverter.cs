using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Spatial.Core.Domain;

namespace Spatial.Core.Infrastructure
{
    public class FeatureConverter<T> : JsonConverter<T>
        where T : BaseEntity
    {
        private readonly JsonSerializer _geometrySerializer = new JsonSerializer();

        public FeatureConverter()
        {
            _geometrySerializer.Converters.Add(new DbGeographyConverter());
        }

        public override void WriteJson(JsonWriter writer, T value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            JObject feature = new JObject();
            feature.Add("type", "Feature");
            feature.Add("id", value.Id);

            var properties = new Dictionary<string, object>();

            foreach (var prop in value.GetType().GetProperties().Where(x => x.Name != "Id"))
            {
                if (typeof(DbGeography).IsAssignableFrom(prop.PropertyType) || typeof(DbGeometry).IsAssignableFrom(prop.PropertyType))
                {
                    feature.Add("geometry", JToken.FromObject(prop.GetValue(value), _geometrySerializer));
                }
                else
                {
                    properties.Add(prop.Name, prop.GetValue(value));
                }
            }

            feature.Add("properties", new JObject(properties.Select(x => new JProperty(x.Key, x.Value))));

            feature.WriteTo(writer);
        }

        public override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
