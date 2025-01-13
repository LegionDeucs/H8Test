namespace NoctisDev.Pooling.Runtime.Pools
{
    public interface IPoolable
    {
        void OnSpawn();
        void OnDespawn();
    }
}