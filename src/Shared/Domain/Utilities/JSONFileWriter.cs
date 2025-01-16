using Domain.Utilities.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Domain.Utilities
{
    internal class JSONFileWriter: IFileWriter
    {
        public async Task WriteData<T>(string path, List<T> data) 
        {
            await using FileStream createStream = File.Create(path);
            await JsonSerializer.SerializeAsync(createStream, data);

            return;
        }
        public async Task<List<T>> ReadData<T>(string path) 
        {
            if (!File.Exists(path)) return new List<T>();
            
            await using FileStream stream = File.OpenRead(path);

            var file_data = await JsonSerializer.DeserializeAsync<List<T>>(stream);
            
            return file_data == null ? file_data : new List<T>();
        }
    }
}
