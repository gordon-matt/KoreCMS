using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Kore.Localization;
using Kore.Web.ContentManagement.Areas.Admin.Media.Models;
using Kore.Web.IO;

namespace Kore.Web.ContentManagement.Areas.Admin.Media.Services
{
    public class FileSystemStorageProvider : IStorageProvider
    {
        private readonly string storagePath;
        private readonly string publicPath;

        public FileSystemStorageProvider(IMediaPathProvider mediaPathProvider)
        {
            storagePath = mediaPathProvider.StoragePath;
            publicPath = mediaPathProvider.PublicPath;
            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.Form["startPath"]))
            {
                var startPath = System.Web.HttpContext.Current.Request.Form["startPath"];
                storagePath = storagePath + "\\" + startPath;
                publicPath = publicPath + "/" + startPath;
            }
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        /// <summary>
        /// Maps a relative path into the storage path.
        /// </summary>
        /// <param name="path">The relative path to be mapped.</param>
        /// <returns>The relative path combined with the storage path.</returns>
        private string MapStorage(string path)
        {
            string mappedPath = string.IsNullOrEmpty(path) ? storagePath : Path.Combine(storagePath, path);
            return PathValidation.ValidatePath(storagePath, mappedPath);
        }

        /// <summary>
        /// Maps a relative path into the public path.
        /// </summary>
        /// <param name="path">The relative path to be mapped.</param>
        /// <returns>The relative path combined with the public path in an URL friendly format ('/' character for directory separator).</returns>
        private string MapPublic(string path)
        {
            return string.IsNullOrEmpty(path) ? publicPath : Path.Combine(publicPath, path).Replace(Path.DirectorySeparatorChar, '/');
        }

        private static string Fix(string path)
        {
            return string.IsNullOrEmpty(path)
                ? ""
                : Path.DirectorySeparatorChar != '/'
                        ? path.Replace('/', Path.DirectorySeparatorChar)
                        : path;
        }

        #region Implementation of IStorageProvider

        /// <summary>
        /// Checks if the given file exists within the storage provider.
        /// </summary>
        /// <param name="path">The relative path within the storage provider.</param>
        /// <returns>True if the file exists; False otherwise.</returns>
        public bool FileExists(string path)
        {
            return File.Exists(MapStorage(path));
        }

        /// <summary>
        /// Retrieves the public URL for a given file within the storage provider.
        /// </summary>
        /// <param name="path">The relative path within the storage provider.</param>
        /// <returns>The public URL.</returns>
        public string GetPublicUrl(string path)
        {
            return MapPublic(path);
        }

        /// <summary>
        /// Retrieves a file within the storage provider.
        /// </summary>
        /// <param name="path">The relative path to the file within the storage provider.</param>
        /// <returns>The file.</returns>
        /// <exception cref="ArgumentException">If the file is not found.</exception>
        public IStorageFile GetFile(string path)
        {
            var fileInfo = new FileInfo(MapStorage(path));
            if (!fileInfo.Exists)
            {
                throw new ArgumentException(string.Format("File {0} does not exist", path));
            }

            return new FileSystemStorageFile(Fix(path), fileInfo);
        }

        /// <summary>
        /// Lists the files within a storage provider's path.
        /// </summary>
        /// <param name="path">The relative path to the folder which files to list.</param>
        /// <returns>The list of files in the folder.</returns>
        public IEnumerable<IStorageFile> ListFiles(string path)
        {
            var directoryInfo = new DirectoryInfo(MapStorage(path));
            if (!directoryInfo.Exists)
            {
                return new List<IStorageFile>();
            }

            return directoryInfo
                .GetFiles()
                .Where(fi => !IsHidden(fi))
                .Select<FileInfo, IStorageFile>(fi => new FileSystemStorageFile(Path.Combine(Fix(path), fi.Name), fi));
        }

        /// <summary>
        /// Checks if the given folder exists within the storage provider.
        /// </summary>
        /// <param name="path">The relative path within the storage provider.</param>
        /// <returns>True if the folder exists; False otherwise.</returns>
        public bool FolderExists(string path)
        {
            return new DirectoryInfo(MapStorage(path)).Exists;
        }

        public IStorageFolder GetFolder(string path)
        {
            var fullPath = MapStorage(path);

            if (Directory.Exists(fullPath))
            {
                return new FileSystemStorageFolder(path, new DirectoryInfo(fullPath));
            }

            return null;
        }

        /// <summary>
        /// Lists the folders within a storage provider's path.
        /// </summary>
        /// <param name="path">The relative path to the folder which folders to list.</param>
        /// <returns>The list of folders in the folder.</returns>
        public IEnumerable<IStorageFolder> ListFolders(string path)
        {
            var directoryInfo = new DirectoryInfo(MapStorage(path));
            if (!directoryInfo.Exists)
            {
                try
                {
                    directoryInfo.Create();
                }
                catch (Exception ex)
                {
                    throw new ArgumentException(string.Format("The folder could not be created at path: {0}. {1}", path, ex));
                }
            }

            return directoryInfo
                .GetDirectories()
                .Where(di => !IsHidden(di))
                .Select<DirectoryInfo, IStorageFolder>(
                    di =>
                        new FileSystemStorageFolder(Path.Combine(Fix(path), di.Name), di));
        }

        /// <summary>
        /// Tries to create a folder in the storage provider.
        /// </summary>
        /// <param name="path">The relative path to the folder to be created.</param>
        /// <returns>True if success; False otherwise.</returns>
        public bool TryCreateFolder(string path)
        {
            try
            {
                // prevent unnecessary exception
                DirectoryInfo directoryInfo = new DirectoryInfo(MapStorage(path));
                if (directoryInfo.Exists)
                {
                    return false;
                }

                CreateFolder(path);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Creates a folder in the storage provider.
        /// </summary>
        /// <param name="path">The relative path to the folder to be created.</param>
        /// <exception cref="ArgumentException">If the folder already exists.</exception>
        public IStorageFolder CreateFolder(string path)
        {
            var directoryInfo = new DirectoryInfo(MapStorage(path));
            if (directoryInfo.Exists)
            {
                throw new ArgumentException(string.Format("Directory {0} already exists", path));
            }

            directoryInfo = Directory.CreateDirectory(directoryInfo.FullName);
            return new FileSystemStorageFolder(path, directoryInfo);
        }

        /// <summary>
        /// Deletes a folder in the storage provider.
        /// </summary>
        /// <param name="path">The relative path to the folder to be deleted.</param>
        /// <exception cref="ArgumentException">If the folder doesn't exist.</exception>
        public bool DeleteFolder(string path)
        {
            var directoryInfo = new DirectoryInfo(MapStorage(path));
            if (directoryInfo.Exists)
            {
                try
                {
                    directoryInfo.Delete(true);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Renames a folder in the storage provider.
        /// </summary>
        /// <param name="oldPath">The relative path to the folder to be renamed.</param>
        /// <param name="newPath">The relative path to the new folder.</param>
        public IStorageFolder RenameFolder(string oldPath, string newPath)
        {
            var sourceDirectory = new DirectoryInfo(MapStorage(oldPath));
            if (!sourceDirectory.Exists)
            {
                throw new ArgumentException(string.Format("Directory {0} does not exist", oldPath));
            }

            var targetDirectory = new DirectoryInfo(MapStorage(newPath));
            if (targetDirectory.Exists)
            {
                throw new ArgumentException(string.Format("Directory {0} already exists", newPath));
            }

            Directory.Move(sourceDirectory.FullName, targetDirectory.FullName);
            return new FileSystemStorageFolder(targetDirectory.FullName, new DirectoryInfo(targetDirectory.FullName));
        }

        /// <summary>
        /// Deletes a file in the storage provider.
        /// </summary>
        /// <param name="path">The relative path to the file to be deleted.</param>
        /// <exception cref="ArgumentException">If the file doesn't exist.</exception>
        public bool DeleteFile(string path)
        {
            var fileInfo = new FileInfo(MapStorage(path));
            if (fileInfo.Exists)
            {
                try
                {
                    fileInfo.Delete();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Renames a file in the storage provider.
        /// </summary>
        /// <param name="oldPath">The relative path to the file to be renamed.</param>
        /// <param name="newPath">The relative path to the new file.</param>
        public IStorageFile RenameFile(string oldPath, string newPath)
        {
            var sourceFileInfo = new FileInfo(MapStorage(oldPath));
            if (!sourceFileInfo.Exists)
            {
                throw new ArgumentException(string.Format("File {0} does not exist", oldPath));
            }

            var targetFileInfo = new FileInfo(MapStorage(newPath));
            if (targetFileInfo.Exists)
            {
                throw new ArgumentException(string.Format("File {0} already exists", newPath));
            }

            File.Move(sourceFileInfo.FullName, targetFileInfo.FullName);
            targetFileInfo = new FileInfo(targetFileInfo.FullName);
            return new FileSystemStorageFile(newPath, targetFileInfo);
        }

        /// <summary>
        /// Creates a file in the storage provider.
        /// </summary>
        /// <param name="path">The relative path to the file to be created.</param>
        /// <exception cref="ArgumentException">If the file already exists.</exception>
        /// <returns>The created file.</returns>
        private IStorageFile CreateFile(string path)
        {
            FileInfo fileInfo = new FileInfo(MapStorage(path));
            if (fileInfo.Exists)
            {
                throw new ArgumentException(string.Format("File {0} already exists", fileInfo.Name));
            }

            // ensure the directory exists
            var dirName = Path.GetDirectoryName(fileInfo.FullName);
            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }
            File.WriteAllBytes(fileInfo.FullName, new byte[0]);

            return new FileSystemStorageFile(Fix(path), fileInfo);
        }

        /// <summary>
        /// Tries to save a stream in the storage provider.
        /// </summary>
        /// <param name="path">The relative path to the file to be created.</param>
        /// <param name="inputStream">The stream to be saved.</param>
        /// <returns>True if success; False otherwise.</returns>
        public bool TrySaveStream(string path, Stream inputStream)
        {
            try
            {
                SaveFile(path, inputStream);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Saves a stream in the storage provider.
        /// </summary>
        /// <param name="path">The relative path to the file to be created.</param>
        /// <param name="inputStream">The stream to be saved.</param>
        /// <exception cref="ArgumentException">If the stream can't be saved due to access permissions.</exception>
        public IStorageFile SaveFile(string path, Stream inputStream)
        {
            // Create the file.
            // The CreateFile method will map the still relative path
            var file = CreateFile(path);

            var outputStream = file.OpenWrite();
            var buffer = new byte[8192];
            for (; ; )
            {
                var length = inputStream.Read(buffer, 0, buffer.Length);
                if (length <= 0)
                    break;
                outputStream.Write(buffer, 0, length);
            }
            outputStream.Dispose();

            file = GetFile(path);

            return file;
        }

        /// <summary>
        /// Combines to paths.
        /// </summary>
        /// <param name="path1">The parent path.</param>
        /// <param name="path2">The child path.</param>
        /// <returns>The combined path.</returns>
        public string Combine(string path1, string path2)
        {
            return Path.Combine(path1, path2);
        }

        private static bool IsHidden(FileSystemInfo di)
        {
            return (di.Attributes & FileAttributes.Hidden) != 0;
        }

        #endregion Implementation of IStorageProvider

        private class FileSystemStorageFile : IStorageFile
        {
            private readonly string path;
            private readonly FileInfo fileInfo;

            public FileSystemStorageFile(string path, FileInfo fileInfo)
            {
                this.path = path;
                this.fileInfo = fileInfo;
            }

            #region Implementation of IStorageFile

            public string GetPath()
            {
                return path;
            }

            public string GetName()
            {
                return fileInfo.Name;
            }

            public long GetSize()
            {
                return fileInfo.Length;
            }

            public DateTime GetLastUpdated()
            {
                return fileInfo.LastWriteTime;
            }

            public string GetFileType()
            {
                return fileInfo.Extension.ToLowerInvariant();
            }

            public Stream OpenRead()
            {
                return new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read);
            }

            public Stream OpenWrite()
            {
                return new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.ReadWrite);
            }

            public Stream CreateFile()
            {
                return new FileStream(fileInfo.FullName, FileMode.Truncate, FileAccess.ReadWrite);
            }

            #endregion Implementation of IStorageFile
        }

        private class FileSystemStorageFolder : IStorageFolder
        {
            private readonly DirectoryInfo directoryInfo;

            public FileSystemStorageFolder(string path, DirectoryInfo directoryInfo)
            {
                Path = path;
                this.directoryInfo = directoryInfo;
            }

            #region IStorageFolder Members

            public string Path { get; }

            public string Name => directoryInfo.Name;

            public DateTime LastUpdated => directoryInfo.LastWriteTime;

            #endregion IStorageFolder Members
        }
    }
}