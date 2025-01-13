using System;
using System.Collections.Generic;
using UnityEngine;

namespace NoctisDev.Pooling.Runtime.Pools
{
    [Serializable]
    public class PoolInfo
    {
        public List<Component> StoredItems;
        public List<Component> SpawnedItems;
        public List<Component> TotalItems;

        public PoolInfo()
        {
            StoredItems = new();
            SpawnedItems = new();
            TotalItems = new();
        }
        
        public PoolInfo(List<Component> storedItems, List<Component> spawnedItems, List<Component> totalItems)
        {
            StoredItems = storedItems;
            SpawnedItems = spawnedItems;
            TotalItems = totalItems;
        }
    }
}