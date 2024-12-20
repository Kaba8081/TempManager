using Domain.Utilities.Interfaces;

namespace Domain.Utilities
{
    internal class CSVFileWriter : IFileWriter
    {
        public void WriteData<T>(string path, T Data) 
        {
            // TODO: Implement
            return;
        }
        public T ReadData<T>(string path)
        {
            // TODO: Implement
            return default(T);
        }
    }
}
