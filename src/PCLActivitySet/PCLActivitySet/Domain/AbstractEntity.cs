using System;

namespace PCLActivitySet.Domain
{
    public abstract class AbstractEntity<TEntity> : AbstractEntity, IEquatable<TEntity>
    {
        public bool Equals(TEntity other)
        {
            return this.Equals(other as AbstractEntity);
        }
    }

    /// <summary>
    /// Base class for entities that use a Guid for purposes of identity.
    /// </summary>
    public abstract class AbstractEntity : IEquatable<AbstractEntity>
    {
        private Guid? _guid;

        /// <summary>
        /// Gets or sets the GUID which serves as the unique identifier for the entity.
        /// </summary>
        public Guid Guid
        {
            get
            {
                if (this._guid == null)
                    this._guid = Guid.NewGuid();
                return this._guid.Value;
            }
            set
            {
                this._guid = value;
            }
        }

        #region Equality, Dictionary key, and helper method(s)

        public override bool Equals(object obj)
        {
            return this.Equals(obj as AbstractEntity);
        }

        public bool Equals(AbstractEntity other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other) || this.Guid == other.Guid)
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            return (this.Guid == default(Guid) ? 561689825 : this.Guid.GetHashCode());
        }

        #endregion
    }
}
