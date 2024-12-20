namespace Domain.Utilities.Interfaces
{
    public interface IFileWriter
    {
        public void WriteData<T>(string path, T data);
        public T? ReadData<T>(string path);
    }
}
