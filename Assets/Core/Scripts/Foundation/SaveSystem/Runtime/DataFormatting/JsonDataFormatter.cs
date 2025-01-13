using System;
using UnityEngine;

namespace MyCore.SaveLoadSystem
{
    public class JsonDataFormatter : IDataFormatter
    {
        private const string FILE_EXTENSION = ".json";

        public string FileExtension => FILE_EXTENSION;
        
        public string Serialize(object data) => 
            JsonUtility.ToJson(data);
        
        public object Deserialize(string serializedData, Type type) => 
            JsonUtility.FromJson(serializedData, type);
    }
}