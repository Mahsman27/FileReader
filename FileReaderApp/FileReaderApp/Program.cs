using System;
using System.IO;
using System.Text;

namespace FileReaderApp
{
    class Program : Processing
    {
        static void Main(string[] args)
        {
            //Writes to the console, reads the input (directory)
            Console.WriteLine("Enter Directory:");
            string directory = Console.ReadLine();

            //Sends the console input (directory) to the ValidateDirectory method
            ValidateDirectory(directory);

            Console.WriteLine("Enter CSV file path:");
            string outputFile = Console.ReadLine();

            ValidateFile(outputFile);

            Processing sendFiles = new Processing();

            sendFiles.DirectoryProcessing(directory, outputFile);
        }

        public static string ValidateDirectory(string directory)
        {
            //Checks to see if the console input exists
            if (Directory.Exists(directory))
            {
                return directory;
            }
            else
            {
                //if the directory does not exist, it prompts the user to try again
                Console.WriteLine("Directory does not exist. Please retry.");
                directory = Console.ReadLine();

                return directory;
            }
        }

        public static string ValidateFile(string outputFile)
        {
            //Checks to see if the specified output file exists
            if (File.Exists(outputFile))
            {
                return outputFile;
            }
            else
            {
                Console.WriteLine("Please enter new file name.");
                outputFile = Console.ReadLine();

                return outputFile;
            }
        }
    }
}
