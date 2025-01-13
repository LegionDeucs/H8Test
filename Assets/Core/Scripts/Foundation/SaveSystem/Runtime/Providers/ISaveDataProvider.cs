using System.Collections.Generic;

namespace MyCore.SaveLoadSystem
{
    public interface ISaveDataProvider
    {
        void SetData<TData>(TData data, string id);
        object GetData(string id);
        Dictionary<string, object> AllData { get; }
    }
}