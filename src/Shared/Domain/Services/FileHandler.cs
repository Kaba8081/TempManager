﻿using Domain.Services.Interfaces;
using Domain.Utilities.Interfaces;
using Domain.Utilities;
using Domain.Models;
using Logger.Utilities;

namespace Domain.Services
{
    public class FileHandler : IFileHandler
    {
        private readonly string _filePath = "";
        private readonly FileFormat _fileFormat = FileFormat.csv;
        private IFileWriter _writer = new CSVFileWriter();

        private string _GetPath(string fileName)
        {
            string path = "";
            path = Path.Combine(_filePath, fileName);
            path += _fileFormat.GetStringValue();
            
            return path;
        }

        public FileHandler() 
        {
            _fileFormat = FileFormat.json;
            _writer = new JSONFileWriter();

            _filePath = Directory.GetCurrentDirectory();
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
        public async Task Save<T>(List<T> data, string fileName)
        {
            var path = _GetPath(fileName);
            if (!File.Exists(path))
            {
                Log.Info("File at given path not found, creating ...");
                try
                {
                    File.Create(path);
                }
                catch (Exception e)
                {
                    Log.Error($"Error creating file at path: {path}, ", e);
                    return;
                }

                Log.Info("File created successfully");
            }

            await _writer.WriteData(_GetPath(fileName), data);
            Log.Info($"Succesfully saved data to {_GetPath(fileName)}");

            return;
        }
        public async Task<List<T>> Read<T>(string fileName)
        {
            var path = _GetPath(fileName);
            if (!File.Exists(path)) 
            {
                Log.Warn($"File at path: {path} not found");
                return new List<T>();
            }

            List<T> data = await _writer.ReadData<T>(path);
            return data;
        }
    }
}
