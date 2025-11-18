using System;
using System.Collections.Generic;

namespace Artifacts
{
    [Serializable]
    public class Artifact : IEqualityComparer<Artifact>
    {
        public DamageType damageType;

        public bool Equals(Artifact x, Artifact y)
        {
            if (x is null || y is null || x.GetType() != y.GetType()) return false;
            if (ReferenceEquals(x, y)) return true;
            return x.damageType == y.damageType;
        }

        public int GetHashCode(Artifact obj)
        {
            return (int)obj.damageType;
        }
    }
}