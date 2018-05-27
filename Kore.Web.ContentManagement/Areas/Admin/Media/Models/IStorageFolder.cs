using System;

namespace Kore.Web.ContentManagement.Areas.Admin.Media.Models
{
    public interface IStorageFolder
    {
        string Path { get; }

        string Name { get; }

        DateTime LastUpdated { get; }
    }

    public class StorageFolder : IStorageFolder
    {
        public string Path { get; set; }

        public string Name { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}