using System;

namespace Common.Models
{
    [Serializable]
    public class FileMessage
    {
        public string FileName { get; set; }
        public byte[] Data { get; set; }
    }
}
