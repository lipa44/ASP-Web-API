using System.Collections.Generic;

namespace Isu.Entities
{
    public class Map<TKey, TValue> : Dictionary<TKey, TValue>
    {
        public KeyCollection GroupNames => Keys;
        public ValueCollection Groups => Values;
    }
}