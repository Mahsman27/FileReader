using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace FileReaderApp
{
    class Processing
    {
        public void DirectoryProcessing(string directory, string outputFile)
        {
            //This will loop through each path in the directory for any file. It will also search any subdirectories using the SearchOption enum.
            foreach (string filePath in Directory.EnumerateFiles(directory, "*", searchOption: SearchOption.AllDirectories))
            {
                //if the path exists then process file
                if (File.Exists(filePath))
                {
                    FileProcessing(filePath, outputFile); //sends path, and outputFile path to be processed under the FileProcessing method. 
                }
                else
                {
                    Console.WriteLine("Path does not exist");
                }

            }
        }

        public void FileProcessing(string filePath, string outputFile)
        {
            string fileSignature = ExtractSignature(filePath); //send the file path to the extract signature method
            string fileType = FileType(fileSignature); //Sends the signature of the file to the FileType method
            string hash = MD5Hash(filePath); //sends the file path to be converted using the MD5 hash algorithm
            CreateCSV(filePath, fileType, hash, outputFile);
        }

        public string ExtractSignature(string filePath)
        {
            int bytesUsed = 4;
            byte[] reader;
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read)) //creates the file stream to read the data
            {
                using BinaryReader binaryReader = new BinaryReader(stream); //creating the binary reader based on the stream
                reader = binaryReader.ReadBytes(bytesUsed); //reads the data of the file
            }
            string fileSignature = BitConverter.ToString(reader); //converts the data that has been read into an array of bytes
            return fileSignature.Replace("-", String.Empty).ToUpper(); //replaces the '-' with nothing, then changes the string to uppercase
        }

        public string FileType(string fileSignature) //Takes the fileSignature as a parameter to check if it's a JPG, PDF or neither. Returns the answer.
        {

            if (fileSignature.StartsWith("FFD8")) //if the file signature starts with FFD8, return JPG
                return "JPG";
            else if (fileSignature.StartsWith("25504446")) //if the file signature starts with 25504446, return PDF
                return "PDF";
            else
                return "Not a JPG or PDF"; //if the file signature isn't a JPG or PDF (the csv file for example) then return 'Not a JPG or PDF'
        }

        public string MD5Hash (string filePath) //Sample MD5 hash builder. I found this technique on stackoverflow, I will confess. I learned something though :)
        {
            MD5 md5hash = MD5.Create();
            byte[] byteArray = md5hash.ComputeHash(Encoding.ASCII.GetBytes(filePath));
            byte[] byteHash = md5hash.ComputeHash(byteArray);
            StringBuilder stringBuilder = new StringBuilder();

            //Converts the byte array to a hexadecimal string
            for (int i = 0; i < byteArray.Length; i++) //loops through byte array
                stringBuilder.Append(byteArray[i].ToString("x2")); //appends the bytes from the byteArray to the string until complete

            return stringBuilder.ToString();
        }


        public void CreateCSV(string filePath, string fileType, string hash, string outputFile)
        {
            string seperator = ","; //csv files use a comma to seperate the columns
            string[][] information = new string[][] { new string[] { filePath, fileType, hash } }; //creates a multi-dimensional array consisting of the file paths, file types, and the hashes
            int lengthOfArray = information.GetLength(0); //grabs the length of the array to use in the loop
            StringBuilder stringBuilder = new StringBuilder(); // creates a new string builder to be able to push information to the csv file

            for (int i = 0; i < lengthOfArray; i++)
                stringBuilder.AppendLine(string.Join(seperator, information[i])); //using the string builder, we can add the information using our seperator

            File.AppendAllText(outputFile, stringBuilder.ToString()); //add the specified information to the output file
        }


    }
}
