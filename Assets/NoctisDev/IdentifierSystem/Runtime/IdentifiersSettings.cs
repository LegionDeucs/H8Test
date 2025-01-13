using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace NoctisDev.IdentifierSystem
{
    [CreateAssetMenu(fileName = "IdentifiersSetting", menuName = "IdentifiersSettings")]
    public class IdentifiersSettings : ScriptableObject
    {
        public List<IdentifierGroup> IdentifierGroups = new();
    }
}