namespace MyCore.SaveLoadSystem
{
    public interface ISaveLoadSystem
    {
        void Save<TData>(string id);
        void SaveAll();
        public TData GetData<TData>(string id);
        void Load();
        void ClearData<TData>(string id);
        void ClearAll();
    }
}