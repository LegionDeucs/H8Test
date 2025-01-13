using System.Collections.Generic;

namespace MyCore.SaveLoadSystem
{
    public abstract class BaseDataRegister : IDataRegister
    {
        public abstract Dictionary<string, RootSaveData> RegisterData();
        
        public TData RegisterData<TData>()
        {
            return new TData();
        }
    }
}