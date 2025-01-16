using Domain.Services.Interfaces;
using Domain.Utilities.Interfaces;
using Domain.Utilities;
using Domain.Models;

namespace Domain.Services
{
    internal class FileHandler : IFileHandler
    {
        private readonly string _filePath = "";
        private readonly string _defaultFileName = "results";
        private readonly FileFormat _fileFormat = FileFormat.csv;
        private IFileWriter _writer = new CSVFileWriter();

        private string _GetPath()
        { 
            return _filePath + _fileFormat.GetStringValue();
        }

        public FileHandler() 
        {
            _fileFormat = FileFormat.json;
            _writer = new JSONFileWriter();

            var _targetPath = Directory.GetCurrentDirectory();
            _filePath = _targetPath + _defaultFileName + _fileFormat.ToString();
        }

        public FileHandler(string filePath)
        {
            _filePath = filePath;
        }
        public FileHandler(string filePath, FileFormat fileFormat) : this(filePath)
        {
            _fileFormat = fileFormat;
            switch (fileFormat) 
            {
                case FileFormat.csv:
                    _writer = new CSVFileWriter();
                    break;
                case FileFormat.json:
                    _writer = new JSONFileWriter();
                    break;
                default:
                    throw new ArgumentException("Unsupported file format", nameof(fileFormat));
            }
        }

        public bool SaveOptions(Dictionary<string, string> options)
        {
            // TODO: Implement
            return false;
        }
        public Dictionary<string, string> ReadOptions()
        {
            // TODO: Implement
            return new Dictionary<string, string> { };
        }
        public bool SaveResults()
        {
            return false;
        }
        public bool ReadResults()
        {
            return false;
        }
    }
}
