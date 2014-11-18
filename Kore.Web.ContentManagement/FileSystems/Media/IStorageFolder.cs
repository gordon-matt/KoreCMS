using System;

namespace Kore.Web.ContentManagement.FileSystems.Media
{
    public interface IStorageFolder
    {
        string GetPath();

        string GetName();

        DateTime GetLastUpdated();
    }

    public class StorageFolder : IStorageFolder
    {
        public string Path { get; set; }

        public string Name { get; set; }

        public DateTime LastUpdated { get; set; }

        public string GetPath()
        {
            return Path;
        }

        public string GetName()
        {
            return Name;
        }

        public DateTime GetLastUpdated()
        {
            return LastUpdated;
        }
    }
}