using CompressionApp.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CompressionApp.Models
{
    class FileService
    {
        public static string WriteFile(string directoryName, string fileName, Byte[] content)
        {
            string fullPath = directoryName + "\\" + fileName + "-Compressed.txt";
            if (File.Exists(fullPath))
                File.Delete(fullPath);
            File.WriteAllBytes(directoryName + "\\" +
                    fileName + "-Compressed.fano", content);
            return fullPath;
        }
    }
}
