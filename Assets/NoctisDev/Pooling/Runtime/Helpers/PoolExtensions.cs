using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NoctisDev.Pooling.Runtime.Helpers
{
    public static class PoolExtensions
    {
        public static Coroutine DelayInvoke(this MonoBehaviour monoBehaviour, float delay, Action action) => 
            monoBehaviour.StartCoroutine(WaitAndDo(delay, action));

        private static IEnumerator WaitAndDo(float delay, Action action)
        {
            yield return new WaitForSeconds(delay);
            action.Invoke();
        }
        
        public static IEnumerable<Type> GetTypesImplementingInterface<T>()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(T).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract);
        }
        
        public static IEnumerable<Type> GetTypesImplementingInterface(Type openGenericInterfaceType)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => type.IsClass && !type.IsAbstract)
                .Where(type => type.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == openGenericInterfaceType));
        }
    }
}