using System;
using System.IO;

namespace FileApp
{
    public class FileOperations
    {
        private static FileOperations? _instance;
        private readonly string _filePath;
        private readonly FileStream _fileStream;
        private readonly StreamWriter _writer;
        private readonly StreamReader _reader;

        // Private constructor prevents external instantiation
        private FileOperations(string filePath)
        {
            _filePath = filePath;
            _fileStream = new FileStream(_filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            _writer = new StreamWriter(_fileStream);
            _reader = new StreamReader(_fileStream);
        }

        // Singleton instance accessor
        public static FileOperations GetInstance(string filePath)
        {
            if (_instance == null)
            {
                _instance = new FileOperations(filePath);
            }
            return _instance;
        }

        public void WriteToFile(string content)
        {
            _writer.WriteLine(content);
            _writer.Flush(); 
        }

        public void ReadFile()
        {
            _fileStream.Seek(0, SeekOrigin.Begin);
            Console.WriteLine("\n--- File Content ---");
            string content;
            while ((content = _reader.ReadLine()) != null)
            {
                Console.WriteLine(content);
            }
        }

        // Ensuring cleanup when execution ends
        public void CloseFile()
        {
            _reader.Close();
            _writer.Close();
            _fileStream.Close();
        }
    }
}
