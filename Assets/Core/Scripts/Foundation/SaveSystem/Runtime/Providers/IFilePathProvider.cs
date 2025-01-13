namespace MyCore.SaveLoadSystem
{
    public interface IFilePathProvider
    {
        string GetFilePath(string id, string fileFormat);
    }
}