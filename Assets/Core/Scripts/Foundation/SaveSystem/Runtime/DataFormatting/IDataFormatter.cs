using System;

namespace MyCore.SaveLoadSystem
{
    public interface IDataFormatter
    {
        string FileExtension { get; }
        string Serialize(object data);
        object Deserialize(string serializedData, Type type);
    }
}