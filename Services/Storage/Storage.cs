using System;
using System.Collections.Generic;
using System.IO;

namespace Storage
{
    public abstract class Storage
    {
        public Storage()
        {
        }

        public abstract Stream Create(string path);

        public abstract TextWriter CreateText(string path);

        public abstract Stream Open(string path);

        public abstract TextReader OpenText(string path);

        public abstract TextWriter AppendText(string path);

        public abstract bool Delete(string path);

        public abstract bool Move(string path, string newPath);

        public abstract bool CreateDir(string path);

        public abstract bool RemoveDir(string path);

        public abstract IEnumerable<string> List(string path, FileAttributes attribute);


        public abstract DateTime GetCreateTime(string path);

        public abstract DateTime GetModifyTime(string path);

        public abstract DateTime GetAccessTime(string path);

        public abstract long GetSize(string path);

        public abstract FileAttributes GetAttributes(string path);


        public FileMeta GetFileMeta(string fullName)
        {
            return new FileMeta()
            {
                FileName = Path.GetFileName(fullName),
                CreateTime = GetCreateTime(fullName),
                ModifyTime = GetModifyTime(fullName),
                AccessTime = GetAccessTime(fullName),
                Attributes = GetAttributes(fullName),
                Size = GetSize(fullName),
            };
        }
    }
}
