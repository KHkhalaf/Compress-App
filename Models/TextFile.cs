using System;
using System.Collections.Generic;
using System.Text;

namespace CompressionApp.Models
{
    class TextFile
    {
        public byte[] Content { get; set; }
        public int Size { get; set; }
        public TextFile(byte[] Content, int Size)
        {
            this.Content = Content;
            this.Size = Size;
        }
    }
}
