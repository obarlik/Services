using System;
using System.IO;

namespace Storage
{
    public abstract class Storage
    {
        public Storage()
        {
        }

        public abstract Stream Create(string path);

        public abstract StreamWriter CreateText(string path);

        public abstract Stream Open(string path, FileMode fileMode);

        public abstract StreamReader OpenText(string path, FileMode fileMode);

        public abstract bool Delete(string path);

        public abstract bool Move(string path, string newPath);

        public abstract bool CreateDir(string path);

        public abstract bool RemoveDir(string path);
    }
}
