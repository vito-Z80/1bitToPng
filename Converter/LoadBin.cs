using System.IO;

namespace _1bitToPng.Converter
{
    public class LoadBin
    {
        const string FileExtension = "*.bin";

        public static string SaveFolder = "Png";
        
        public byte[] GetFileBytes(string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException($"\nFile '{fileName}' not found. \nThe file must be in the local folder");
            }

            var bytes = File.ReadAllBytes(fileName);
            return bytes;
        }


        public string[] GetLocalBinFiles()
        {
            var dir = Directory.GetCurrentDirectory();
            var files = Directory.GetFiles(dir, FileExtension);
            return files;
        }
    }
}