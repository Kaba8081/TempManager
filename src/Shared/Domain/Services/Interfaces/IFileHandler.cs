namespace Domain.Services.Interfaces
{
    public interface IFileHandler
    {
        Task Save<T>(List<T> data, string filePath);
        Task<List<T>> Read<T>(string filePath);
    }
}
