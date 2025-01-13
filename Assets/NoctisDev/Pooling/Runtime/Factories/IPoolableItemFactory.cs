using UnityEngine;

namespace NoctisDev.Pooling.Runtime.Factories
{
    public interface IPoolableItemFactory<out T> where T : Component
    {
        T Prefab { get; }
        T Create(Transform parent);
    }
}