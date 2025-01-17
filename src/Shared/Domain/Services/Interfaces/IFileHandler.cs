namespace Domain.Services.Interfaces
{
    public interface IFileHandler
    {
        bool SaveOptions(Dictionary<string, string> data);
        Dictionary<string, string> ReadOptions();
        Task Save<T>(List<T> data, string filePath);
        Task<List<T>> Read<T>(string filePath);
    }
}
