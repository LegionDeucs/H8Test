using NoctisDev.Pooling.Runtime.Pools;
using UnityEngine;

namespace NoctisDev.Pooling.Runtime.Behaviours
{
    public class PoolObject : MonoBehaviour
    {
        [SerializeField] private PoolInfo _info;

        public void Construct(IPool pool)
        {
            _info = pool.Info;
        }
    }
}