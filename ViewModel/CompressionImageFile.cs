using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace CompressionApp.Models
{
    public static class CompressionImageFile
    {
        public static List<byte> listofBytes = new List<byte>();
        public static string Compress(FileStream inputFile, string fileName,string Oldfile)
        {
            int dictSize = 256;
            string P = "", BP = "";
            byte[] buffer = new byte[3];
            char C;
            bool isLeft = true;
            int i, byteToInt;
            Dictionary<string, int> dictionary = new Dictionary<string, int>();

            // Character dictionary 
            for (i = 0; i < 256; i++)
            {
                dictionary.Add(((char)i).ToString(), i);
            }

            Init(Oldfile);
            try
            {

                // Read first byte to initialize P
                byteToInt = inputFile.ReadByte();

                C = (char)byteToInt;
                P = "" + C;
                while (true)
                {
                    byteToInt = inputFile.ReadByte();
                    if (byteToInt == -1)
                        throw new IOException(); 
                    C = (char)byteToInt;

                    // if P+C is present in dictionary
                    if (dictionary.ContainsKey(P + C))
                    {
                        P = P + C;
                    }

                    else
                    {
                        BP = convertTo12Bit(dictionary[P]);
                        if (isLeft)
                        {
                            buffer[0] = (byte)Convert.ToInt32(BP.Substring(0, 8),2);
                            buffer[1] = (byte)Convert.ToInt32(BP.Substring(8, 4) + "0000",2);
                        }
                        else
                        {
                            buffer[1] += (byte)Convert.ToInt32(BP.Substring(0, 4),2);
                            buffer[2] = (byte)Convert.ToInt32(BP.Substring(4, 8),2);

                            for (i = 0; i < buffer.Length; i++)
                            {
                                listOfBytes.Add(buffer[i]);
                                buffer[i] = 0;
                            }
                        }
                        isLeft = !isLeft;
                        if (dictSize < 4096) dictionary.Add(P + C, dictSize++);

                        P = "" + C;
                    }
                }

            }
            catch (IOException)
            {
                BP = convertTo12Bit(dictionary[P]);
                if (isLeft)
                {
                    buffer[0] = (byte)Convert.ToInt32(BP.Substring(0, 8),2);
                    buffer[1] = (byte)Convert.ToInt32(BP.Substring(8, 4) + "0000",2);
                    listOfBytes.Add(buffer[0]);
                    listOfBytes.Add(buffer[1]);
                }
                else
                {
                    buffer[1] += (byte)Convert.ToInt32(BP.Substring(0, 4),2);
                    buffer[2] = (byte)Convert.ToInt32(BP.Substring(4, 8),2);
                    for (i = 0; i < buffer.Length; i++)
                    {
                        listOfBytes.Add(buffer[i]);
                        buffer[i] = 0;
                    }
                }
                byte[] AllBytes = listOfBytes.ToArray();
                File.WriteAllBytes(fileName.Substring(0, fileName.Length - 4) + "-compressed.lzw", AllBytes);
                inputFile.Close();
                return fileName.Substring(0, fileName.Length - 4) + "-compressed.lzw";
            }

        }

        public static string convertTo12Bit(int x)
        {
            string to12Bit = Convert.ToString(x, 2);
            while (to12Bit.Length < 12) to12Bit = "0" + to12Bit;
            return to12Bit;
        }


        // Decompression File

        public static List<byte> listOfBytes;
        public static string Decompress(FileStream inputFile,string filename)
        {
            string[] arrayOfChar;
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            int dictSize = 256;
            byte[] buffer = new byte[3];
            bool isLeft = true;
            int currentword, previousword, i;

            arrayOfChar = new string[4096];

            for (i = 0; i < 256; i++) arrayOfChar[i] = ((char)i).ToString();

            // Read input file and output file

            Init(inputFile);
            int b1 = inputFile.ReadByte();
            int b2 = inputFile.ReadByte();
            buffer[0] = Convert.ToByte(b1);
            buffer[1] = Convert.ToByte(b2);
            previousword = getIntValue(buffer[0], buffer[1], isLeft);
            isLeft = !isLeft;
            //  check UTF8Encoding may has error
            byte[] info = Encoding.UTF8.GetBytes(arrayOfChar[previousword]);
            listOfBytes.AddRange(info);

            // Reads three bytes and generates corresponding characters
            while (true)
            {

                if (isLeft)
                {
                    if(inputFile.Position<inputFile.Length)
                        buffer[0] = Convert.ToByte(inputFile.ReadByte());
                    else
                        break;
                    if (inputFile.Position < inputFile.Length)
                        buffer[1] = Convert.ToByte(inputFile.ReadByte());
                    else
                        break;
                    currentword = getIntValue(buffer[0], buffer[1], isLeft);
                }
                else
                {
                    if (inputFile.Position < inputFile.Length)
                        buffer[2] = Convert.ToByte(inputFile.ReadByte());
                    else
                        break;
                    currentword = getIntValue(buffer[1], buffer[2], isLeft);
                }
                isLeft = !isLeft;

                if (currentword >= dictSize)
                {
                    if (dictSize < 4096)
                    {
                        arrayOfChar[dictSize] = arrayOfChar[previousword] + arrayOfChar[previousword].Substring(0,1);
                    }
                    dictSize++;
                    info = Encoding.UTF8.GetBytes(arrayOfChar[previousword] + arrayOfChar[previousword].Substring(0, 1));
                    listOfBytes.AddRange(info);
                }

                else
                {
                    if (dictSize < 4096)
                    {
                        arrayOfChar[dictSize] = 
                            arrayOfChar[previousword] + 
                            arrayOfChar[currentword].Substring(0, 1);
                    }
                    dictSize++;
                    info = Encoding.UTF8.GetBytes(arrayOfChar[currentword]);
                    listOfBytes.AddRange(info);
                }
                previousword = currentword;
            }
            byte[] imageBytes = listofBytes.ToArray();

            using (MemoryStream memstr = new MemoryStream(imageBytes))
            {
                Image img = Image.FromStream(memstr);
                img.Save(filename.Substring(0, filename.Length - 15) + "-decompressed.png");
                inputFile.Close(); 
                return filename.Substring(0, filename.Length - 15) + "-decompressed.png";
            }
        }
        
        public static int getIntValue(byte b1, byte b2, bool isLeft)
        {
            string t1 = Convert.ToString(b1, 2);
            string t2 = Convert.ToString(b2, 2);

            while (t1.Length < 8) t1 = "0" + t1;
            if (t1.Length == 32) t1 = t1.Substring(24, 8);

            while (t2.Length < 8) t2 = "0" + t2;
            if (t2.Length == 32) t2 = t2.Substring(24, 8);
            if (isLeft) 
                return Convert.ToInt16(t1 + t2.Substring(0, 4), 2);
            else 
                return Convert.ToInt16(t1.Substring(4, 4) + t2, 2);

        }
        public static void Init(FileStream inputFile)
        {
            listOfBytes = new List<byte>();
            int size = inputFile.ReadByte();
            for (int i = 0; i < size; i++)
                listOfBytes.Add((byte)inputFile.ReadByte());
            string fN = Encoding.ASCII.GetString(listOfBytes.ToArray());
            listofBytes = new List<byte>();
            listofBytes.AddRange(File.ReadAllBytes(fN));
        }  
        public static void Init(string fileName)
        {
            listOfBytes = new List<byte>();
            byte[] fN = Encoding.ASCII.GetBytes(fileName);
            listOfBytes.Add((byte)fN.Length);
            listOfBytes.AddRange(fN);
        }
    }
}
