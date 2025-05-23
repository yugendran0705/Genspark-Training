using System;

namespace FileApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath = "sample.txt";
            FileOperations fileOps = FileOperations.GetInstance(filePath);

            Console.WriteLine("Enter text to write into the file (type 'exit' to stop):");
            while (true)
            {
                string input = Console.ReadLine() ?? "";
                if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    break;
                fileOps.WriteToFile(input);
            }

            // Reading file content
            fileOps.ReadFile();

            // Closing file
            fileOps.CloseFile();

            Console.WriteLine("\nFile operations completed successfully.");
        }
    }
}
