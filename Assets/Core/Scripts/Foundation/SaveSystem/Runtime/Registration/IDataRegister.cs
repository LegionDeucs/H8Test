using System.Collections.Generic;

namespace MyCore.SaveLoadSystem
{
    public interface IDataRegister
    {
        Dictionary<string, RootSaveData> RegisterData();
        TData RegisterData<TData>();
    }
}