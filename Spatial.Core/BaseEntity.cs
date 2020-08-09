using System.Diagnostics.CodeAnalysis;

namespace Spatial.Core
{
    /// <summary>
    /// Base class for entities
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        public string Id { get; set; }

        public static bool operator ==(BaseEntity x, BaseEntity y)
        {
            return object.Equals(x, y);
        }

        public static bool operator !=(BaseEntity x, BaseEntity y)
        {
            return !object.Equals(x, y);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            BaseEntity rhs = obj as BaseEntity;
            return this.Id == rhs.Id;
        }

        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            var hashCode = this.GetType().GetHashCode();
            return (hashCode * 31) ^ Id.GetHashCode();
        }

    }
}
