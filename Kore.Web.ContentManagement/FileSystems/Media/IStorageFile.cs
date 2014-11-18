using System;
using System.IO;

namespace Kore.Web.ContentManagement.FileSystems.Media
{
    public interface IStorageFile
    {
        string GetPath();

        string GetName();

        long GetSize();

        DateTime GetLastUpdated();

        string GetFileType();

        /// <summary>
        /// Creates a stream for reading from the file.
        /// </summary>
        Stream OpenRead();

        /// <summary>
        /// Creates a stream for writing to the file.
        /// </summary>
        Stream OpenWrite();

        /// <summary>
        /// Creates a stream for writing to the file, and truncates the existing content.
        /// </summary>
        Stream CreateFile();
    }

    public class StorageFile : IStorageFile
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public long Size { get; set; }

        public DateTime LastUpdated { get; set; }

        public string FileType { get; set; }

        public string GetPath()
        {
            return Path;
        }

        public string GetName()
        {
            return Name;
        }

        public long GetSize()
        {
            return Size;
        }

        public DateTime GetLastUpdated()
        {
            return LastUpdated;
        }

        public string GetFileType()
        {
            return FileType;
        }

        public Stream OpenRead()
        {
            throw new NotSupportedException();
        }

        public Stream OpenWrite()
        {
            throw new NotSupportedException();
        }

        public Stream CreateFile()
        {
            throw new NotSupportedException();
        }
    }
}