namespace Domain.Utilities.Interfaces
{
    public interface IFileWriter
    {
        public Task WriteData<T>(string path, List<T> data);
        public Task<List<T?>> ReadData<T>(string path);
    }
}
