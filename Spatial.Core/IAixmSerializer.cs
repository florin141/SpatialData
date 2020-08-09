using System.IO;

namespace Spatial.Core
{
    public interface IAixmSerializer<T>
    {
        T Deserialize(string input);

        T Deserialize(Stream stream);

        string Serialize(T obj, bool namespacePrefixes = false);
    }
}
