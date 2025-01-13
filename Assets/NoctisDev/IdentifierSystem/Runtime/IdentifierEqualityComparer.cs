using System.Collections.Generic;

namespace NoctisDev.IdentifierSystem
{
    public class IdentifierEqualityComparer : IEqualityComparer<Identifier>
    {
        public bool Equals(Identifier x, Identifier y) => 
            x.Value == y.Value;

        public int GetHashCode(Identifier id) => 
            id.Value.GetHashCode();
    }
}