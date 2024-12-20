using Domain.Utilities.Interfaces;

namespace Domain.Utilities
{
    internal class JSONFileWriter: IFileWriter
    {
        public void WriteData<T>(string path, T data) 
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
