using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Storage
{
    public class FileMeta
    {
        public FileMeta()
        {
        }

        public string FileName { get; set; }

        public FileAttributes Attributes { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime ModifyTime { get; set; }

        public DateTime AccessTime { get; set; }

        public long Size { get; set; }
    }
}
