using System;

namespace NoctisDev.Pooling.Runtime.Helpers
{
    public interface IDelayedInvoker
    {
        void DelayedInvoke(float delay, Action action);
    }
}