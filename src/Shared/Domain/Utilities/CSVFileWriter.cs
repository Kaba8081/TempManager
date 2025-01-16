using Domain.Utilities.Interfaces;

namespace Domain.Utilities
{
    internal class CSVFileWriter : IFileWriter
    {
        public Task WriteData<T>(string path, List<T> Data) 
        {
            // TODO: Implement
            
            return Task.CompletedTask;
        }
        public async Task<List<T>> ReadData<T>(string path)
        {
            // TODO: Implement
            await Task.CompletedTask;
            return [];
        }
    }
}
