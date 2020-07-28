using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Shapes;
using System.IO;
using CompressionApp.ViewModel;
using CompressionApp.Models;
using System.Text;
using System.Collections.Generic;
using System.Drawing;
using Ookii.Dialogs.Wpf;
using System.Drawing.Imaging;
using System.Security.Policy;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Threading.Tasks;

namespace CompressionApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CompressTextFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Text files (*.txt)|*.txt";
            openFileDialog.InitialDirectory = @"C:\Users\hp\Desktop";
            if (openFileDialog.ShowDialog() == true)
            {
                var fileInfo = new FileInfo(openFileDialog.FileName);
                sizeBefore.Content = fileInfo.Length + " Byte";
                string content = File.ReadAllText(openFileDialog.FileName);
                textBox.Text = content;
                TextFile compressedFile = CompressionTextFile.CompressTextFile(content);
                sizeAfter.Content = compressedFile.Size + " Byte";
                string directoryName = fileInfo.DirectoryName.ToString();
                string fileName = fileInfo.Name.Substring(0, fileInfo.Name.Length - 4);
                filePathlbl.Content = "file path : " + FileService.WriteFile(directoryName, fileName, compressedFile.Content);
                textBox.Visibility = Visibility.Visible;
                imageSelected.Visibility = Visibility.Hidden;
            }
        }
        private void CompressImageFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.png, *.jpg,*.bmp)|*.png;*.bmp;*.jpg;";
            openFileDialog.InitialDirectory = @"C:\Users\hp\Desktop";
            if (openFileDialog.ShowDialog() == true)
            {
                FileStream selectedFile = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read);

                var fileInfo = new FileInfo(openFileDialog.FileName);
                sizeBefore.Content = fileInfo.Length + " Byte";
                string path = CompressionImageFile.Compress(selectedFile, openFileDialog.FileName, openFileDialog.FileName);

                fileInfo = new FileInfo(path);
                sizeAfter.Content = fileInfo.Length + " Byte";

                imageSelected.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                imageSelected.Visibility = Visibility.Visible;
                textBox.Visibility = Visibility.Hidden;
                filePathlbl.Content = "file path : " + path;
            }
        }
        private void CompressFolder(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog FolderDialog = new VistaFolderBrowserDialog();

            if ((bool)FolderDialog.ShowDialog(this))
            { 
                string[] files = Directory.GetFiles(FolderDialog.SelectedPath);
                string newDirectory = FolderDialog.SelectedPath + "-compressed";
                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i].Contains(".png") || files[i].Contains(".jpg") || files[i].Contains(".bmp"))
                    {
                        Directory.CreateDirectory(newDirectory);
                        FileStream selectedFile = new FileStream(files[i], FileMode.Open, FileAccess.Read);
                        int index = files[i].LastIndexOf("\\");
                        string fileName = files[i].Substring(index);
                        CompressionImageFile.Compress(selectedFile, newDirectory + fileName, files[i]);

                    }
                    else if (files[i].Contains(".txt"))
                    {
                        string content = File.ReadAllText(files[i]);
                        TextFile compressedFile = CompressionTextFile.CompressTextFile(content);
                        var fileInfo = new FileInfo(files[i]);
                        string fileName = fileInfo.Name.Substring(0, fileInfo.Name Length - 4);
                        FileService.WriteFile(newDirectory, fileName, compressedFile.Content);
                    }
                }
                filePathlbl.Content = "file path : " + newDirectory;
            }
        }
        private void DecompressTextFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.fano)|*.fano"; 
            openFileDialog.InitialDirectory = @"C:\Users\hp\Desktop";
            if (openFileDialog.ShowDialog() == true)
            {
                byte[] content = File.ReadAllBytes(openFileDialog.FileName);
                KeyValuePair<int, TextFile> decompressTextFile = CompressionTextFile.DecompressTextFile(content);
                TextFile compressedFile = decompressTextFile.Value;
                sizeBefore.Content = compressedFile.Size + " Byte";
                string _content = Encoding.UTF8.GetString(compressedFile.Content, 0, compressedFile.Content.Length);
                textBox.Text = _content;
                File.WriteAllText(openFileDialog.FileName.Substring(0, openFileDialog.FileName.Length - 15)+".txt", _content);
                sizeAfter.Content = decompressTextFile.Key + " Byte";
               
                filePathlbl.Content = "file path : " + openFileDialog.FileName.Substring(0, openFileDialog.FileName.Length - 15) + ".txt";
                textBox.Visibility = Visibility.Visible;
                imageSelected.Visibility = Visibility.Hidden;
            }
        }
        private void DeCompressImageFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Compressed Files(.lzw)|*.lzw;";
            openFileDialog.InitialDirectory = @"C:\Users\hp\Desktop";
            if (openFileDialog.ShowDialog() == true)
            {
                FileStream selectedFile = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read);

                var fileInfo = new FileInfo(openFileDialog.FileName);
                sizeBefore.Content = fileInfo.Length + " Byte";
                string path = CompressionImageFile.Decompress(selectedFile, openFileDialog.FileName);
                filePathlbl.Content = "file path : " + path;
                fileInfo = new FileInfo(path);
                sizeAfter.Content = fileInfo.Length + " Byte";

                imageSelected.Source = new BitmapImage(new Uri(path));
                textBox.Visibility = Visibility.Hidden;
                imageSelected.Visibility = Visibility.Visible;
            }
        }
        private void DeCompressFolder(object sender, RoutedEventArgs e)
        {

            VistaFolderBrowserDialog FolderDialog = new VistaFolderBrowserDialog();

            if ((bool)FolderDialog.ShowDialog(this))
            {
                string[] files = Directory.GetFiles(FolderDialog.SelectedPath);
                string newDirectory = FolderDialog.SelectedPath.Substring(0, FolderDialog.SelectedPath.Length - 11) + "-decompressed";
                for (int i = 0; i < files.Length; i++)
                {
                    if (files[i].Contains(".lzw"))
                    {
                        Directory.CreateDirectory(newDirectory);
                        FileStream selectedFile = new FileStream(files[i], FileMode.Open, FileAccess.Read);
                        int index = files[i].LastIndexOf("\\");
                        string fileName = files[i].Substring(index);
                        CompressionImageFile.Decompress(selectedFile, newDirectory + fileName);

                    }
                    else if (files[i].Contains(".fano"))
                    {
                        byte[] content = File.ReadAllBytes(files[i]);
                        KeyValuePair<int, TextFile> decompressTextFile = CompressionTextFile.DecompressTextFile(content);
                        int index = files[i].LastIndexOf("\\");
                        string fileName = files[i].Substring(index, files[i].Length - index - 5);
                        string _content = Encoding.UTF8.GetString(decompressTextFile.Value.Content, 0, decompressTextFile.Value.Content.Length);

                        File.WriteAllText(newDirectory + fileName + ".txt", _content);
                    }
                }
                filePathlbl.Content = "file path : " + newDirectory;
            }
        }
        private void CompressImageUsingJpegLossy(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.png, *.jpg,*.bmp)|*.png;*.bmp;*.jpg;";
            openFileDialog.InitialDirectory = @"C:\Users\hp\Desktop";
            if (openFileDialog.ShowDialog() == true)
            {
                var fileInfo = new FileInfo(openFileDialog.FileName);
                sizeBefore.Content = fileInfo.Length + " Byte";
                string newPath;
                using (var image = Image.FromFile(openFileDialog.FileName))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        image.Save(ms, ImageFormat.Jpeg);
                    }
                    newPath = @"F:\Descover_Your_self_" + Guid.NewGuid() + ".jpeg";
                    image.Save(newPath);
                }
                textBox.Visibility = Visibility.Hidden;
                imageSelected.Visibility = Visibility.Visible;
                imageSelected.Source = new BitmapImage(new Uri(newPath));
                fileInfo = new FileInfo(newPath);
                sizeAfter.Content = fileInfo.Length + " Byte";
                filePathlbl.Content = "file path : " + newPath;
            }
        }
    }
}
