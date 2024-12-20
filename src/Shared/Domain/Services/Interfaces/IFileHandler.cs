namespace Domain.Services.Interfaces
{
    internal interface IFileHandler
    {
        bool SaveOptions(Dictionary<string, string> data);
        Dictionary<string, string> ReadOptions();
        bool SaveResults();
        bool ReadResults(); // TODO: Add Result type
    }
}
