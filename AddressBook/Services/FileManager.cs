using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace AddressBook.Services
{
    internal interface IFileManager
    {
        public void Save(string filePath, string text);
        public string Read(string filePath);
    }
    internal class FileManager : IFileManager
    {
        public void Save(string filePath, string text)
        {
            try
            {
                using var sw = new StreamWriter(filePath);
                sw.WriteLine(text);
            }
            catch
            {
                Console.Clear();
                Console.WriteLine("Unable to save the product catalog");
                Console.ReadKey();
            }
        }

        public string Read(string filePath)
        {
            try
            {
                using var sr = new StreamReader(filePath);
                return sr.ReadToEnd();
            }
            catch { }

            return "[]";
        }       
    }
}
