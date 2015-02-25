using System;
using System.IO;

namespace ElFinder
{
    public class FullPath
    {
        private FileSystemInfo fileSystemObject;

        public FullPath(Root root, FileSystemInfo fileSystemObject)
        {
            if (root == null)
            {
                throw new ArgumentNullException("root", "Root can not be null");
            }
            if (fileSystemObject == null)
            {
                throw new ArgumentNullException("root", "Filesystem object can not be null");
            }

            Root = root;
            this.fileSystemObject = fileSystemObject;
            IsDirectory = this.fileSystemObject is DirectoryInfo;

            string trimmedfileSystemObjectName = fileSystemObject.FullName.Trim(new[] { '\\' });
            string trimmedRoot = root.Directory.FullName.Trim(new[] { '\\' });

            if (trimmedfileSystemObjectName.StartsWith(trimmedRoot))
            {
                if (trimmedfileSystemObjectName.Length == trimmedRoot.Length)
                {
                    RelativePath = string.Empty;
                }
                else
                {
                    RelativePath = trimmedfileSystemObjectName.Substring(trimmedRoot.Length + 1);
                }
            }
            else
            {
                throw new InvalidOperationException("Filesystem object must be in it root directory or in root subdirectory");
            }
        }

        public DirectoryInfo Directory
        {
            get { return IsDirectory ? (DirectoryInfo)fileSystemObject : null; }
        }

        public FileInfo File
        {
            get { return !IsDirectory ? (FileInfo)fileSystemObject : null; }
        }

        public bool IsDirectory { get; private set; }

        public string RelativePath { get; private set; }

        public Root Root { get; private set; }
    }
}