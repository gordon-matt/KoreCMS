//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using ElFinder;
//using ElFinder.DTO;
//using ElFinder.Response;
//using Kore.Web.ContentManagement.FileSystems.Media;
//using Kore.Web.Mvc;

//namespace Kore.Web.ContentManagement.Areas.Admin.Media.Services
//{
//    public class SystemMediaDriver : IDriver
//    {
//        private readonly IStorageProvider storageProvider;
//        private readonly IMimeTypeProvider mimeTypeProvider;
//        private static readonly DateTime unixOrigin = new DateTime(1970, 1, 1, 0, 0, 0);
//        private readonly Lazy<IImageService> mediaService;

//        public SystemMediaDriver(IStorageProvider storageProvider, IMimeTypeProvider mimeTypeProvider, Lazy<IImageService> mediaService)
//        {
//            this.storageProvider = storageProvider;
//            this.mimeTypeProvider = mimeTypeProvider;
//            this.mediaService = mediaService;
//        }

//        private static JsonResult Json(object data)
//        {
//            return new JsonNetResult(data);
//            //return new JsonDataContractResult(data) { JsonRequestBehavior = JsonRequestBehavior.AllowGet, ContentType = "text/html" };
//        }

//        public JsonResult Open(string target, bool tree)
//        {
//            if (string.IsNullOrEmpty(target))
//            {
//                return Init(target);
//            }

//            string path = Helper.DecodePath(target).Trim(Path.DirectorySeparatorChar);

//            if (string.IsNullOrEmpty(path))
//            {
//                return Init(target);
//            }

//            var folder = storageProvider.GetFolder(path);
//            var subFolders = storageProvider.ListFolders(path);
//            var files = storageProvider.ListFiles(path);

//            var response = new OpenResponse(CreateDTO(folder, Path.GetDirectoryName(path)));

//            foreach (var file in files)
//            {
//                response.AddResponse(CreateDTO(file, path));
//            }

//            foreach (var subFolder in subFolders)
//            {
//                response.AddResponse(CreateDTO(subFolder, path));
//            }
//            return Json(response);
//        }

//        public JsonResult Init(string target)
//        {
//            var folders = storageProvider.ListFolders(null);
//            var files = storageProvider.ListFiles(null);

//            var directory = new RootDTO
//            {
//                Mime = "directory",
//                Dirs = folders.Any() ? (byte)1 : (byte)0,
//                Hash = Helper.EncodePath("\\"),
//                Locked = 0,
//                Name = "Media",
//                Read = 1,
//                Write = 1,
//                Size = 0,
//                UnixTimeStamp = (long)(DateTime.UtcNow - unixOrigin).TotalSeconds,
//                VolumeId = "v1"
//            };

//            var response = new InitResponse(directory);

//            foreach (var file in files)
//            {
//                response.AddResponse(CreateDTO(file, null));
//            }

//            foreach (var folder in folders)
//            {
//                response.AddResponse(CreateDTO(folder, null));
//            }

//            response.Options.Path = "Media";
//            response.Options.Url = "/";
//            response.Options.ThumbnailsUrl = "/";

//            return Json(response);
//        }

//        public JsonResult Parents(string target)
//        {
//            //string path = Helper.DecodePath(target);
//            //var folders = mediaService.Value.GetMediaFolders(target);
//            //var response = new TreeResponse();

//            //foreach (var folder in folders)
//            //{
//            //    response.Tree.Add(folder);
//            //}

//            //return Json(response);
//            throw new NotSupportedException();

//            //var fullPath = ParsePath(target);
//            //var answer = new TreeResponse();
//            //if (fullPath.Directory.FullName == fullPath.Root.Directory.FullName)
//            //{
//            //    answer.Tree.Add(DTOBase.Create(fullPath.Directory, fullPath.Root));
//            //}
//            //else
//            //{
//            //    DirectoryInfo parent = fullPath.Directory;
//            //    foreach (var item in parent.Parent.GetDirectories())
//            //    {
//            //        answer.Tree.Add(DTOBase.Create(item, fullPath.Root));
//            //    }
//            //    while (parent.FullName != fullPath.Root.Directory.FullName)
//            //    {
//            //        parent = parent.Parent;
//            //        answer.Tree.Add(DTOBase.Create(parent, fullPath.Root));
//            //    }
//            //}
//            //return Json(answer);
//        }

//        public JsonResult Tree(string target)
//        {
//            var path = Helper.DecodePath(target).Trim(Path.DirectorySeparatorChar);
//            var parentPath = string.IsNullOrEmpty(path) ? null : Path.GetDirectoryName(path);
//            var folders = storageProvider.ListFolders(path);
//            var answer = new TreeResponse();
//            foreach (var folder in folders)
//            {
//                answer.Tree.Add(CreateDTO(folder, parentPath));
//            }
//            return Json(answer);
//        }

//        public JsonResult List(string target)
//        {
//            string path = Helper.DecodePath(target);
//            var files = storageProvider.ListFiles(path);
//            var response = new ListResponse();

//            foreach (var file in files)
//            {
//                response.List.Add(file.GetName());
//            }

//            return Json(response);
//        }

//        public JsonResult MakeDir(string target, string name)
//        {
//            var path = Helper.DecodePath(target).Trim(Path.DirectorySeparatorChar);
//            var folder = storageProvider.CreateFolder(storageProvider.Combine(path, name));
//            return Json(new AddResponse(CreateDTO(folder, path)));
//        }

//        public JsonResult MakeFile(string target, string name)
//        {
//            FullPath fullPath = ParsePath(target);
//            FileInfo newFile = new FileInfo(Path.Combine(fullPath.Directory.FullName, name));
//            newFile.Create().Close();
//            return Json(new AddResponse(newFile, fullPath.Root));
//        }

//        public JsonResult Rename(string target, string name)
//        {
//            var path = Helper.DecodePath(target);
//            var folderPath = Path.GetDirectoryName(path);
//            var answer = new ReplaceResponse();

//            if (storageProvider.FileExists(path))
//            {
//                var oldFileName = Path.GetFileName(path);
//                var mediaFile = storageProvider.RenameFile(storageProvider.Combine(folderPath, oldFileName), storageProvider.Combine(folderPath, name));
//                answer.Removed.Add(target);
//                answer.Added.Add(CreateDTO(mediaFile, path));
//            }
//            else
//            {
//                var mediaFolder = storageProvider.GetFolder(path);
//                if (mediaFolder != null)
//                {
//                    mediaFolder = storageProvider.RenameFolder(path, name);

//                    answer.Removed.Add(target);
//                    answer.Added.Add(CreateDTO(mediaFolder, folderPath));
//                }
//            }

//            return Json(answer);
//        }

//        public JsonResult Remove(IEnumerable<string> targets)
//        {
//            var answer = new RemoveResponse();
//            foreach (var item in targets)
//            {
//                var path = Helper.DecodePath(item);

//                if (storageProvider.FileExists(path))
//                {
//                    storageProvider.DeleteFile(path);
//                }
//                else if (storageProvider.FolderExists(path))
//                {
//                    storageProvider.DeleteFolder(path);
//                }

//                answer.Removed.Add(item);
//            }
//            return Json(answer);
//        }

//        public JsonResult Duplicate(IEnumerable<string> targets)
//        {
//            throw new NotImplementedException();

//            //AddResponse response = new AddResponse();
//            //foreach (var target in targets)
//            //{
//            //    FullPath fullPath = ParsePath(target);
//            //    if (fullPath.Directory != null)
//            //    {
//            //        var parentPath = fullPath.Directory.Parent.FullName;
//            //        var name = fullPath.Directory.Name;
//            //        var newName = string.Format(@"{0}\{1} copy", parentPath, name);
//            //        if (!Directory.Exists(newName))
//            //        {
//            //            DirectoryCopy(fullPath.Directory, newName, true);
//            //        }
//            //        else
//            //        {
//            //            for (int i = 1; i < 100; i++)
//            //            {
//            //                newName = string.Format(@"{0}\{1} copy {2}", parentPath, name, i);
//            //                if (!Directory.Exists(newName))
//            //                {
//            //                    DirectoryCopy(fullPath.Directory, newName, true);
//            //                    break;
//            //                }
//            //            }
//            //        }
//            //        response.Added.Add(DTOBase.Create(new DirectoryInfo(newName), fullPath.Root));
//            //    }
//            //    else
//            //    {
//            //        var parentPath = fullPath.File.Directory.FullName;
//            //        var name = fullPath.File.Name.Substring(0, fullPath.File.Name.Length - fullPath.File.Extension.Length);
//            //        var ext = fullPath.File.Extension;

//            //        var newName = string.Format(@"{0}\{1} copy{2}", parentPath, name, ext);

//            //        if (!File.Exists(newName))
//            //        {
//            //            fullPath.File.CopyTo(newName);
//            //        }
//            //        else
//            //        {
//            //            for (int i = 1; i < 100; i++)
//            //            {
//            //                newName = string.Format(@"{0}\{1} copy {2}{3}", parentPath, name, i, ext);
//            //                if (!File.Exists(newName))
//            //                {
//            //                    fullPath.File.CopyTo(newName);
//            //                    break;
//            //                }
//            //            }
//            //        }
//            //        response.Added.Add(DTOBase.Create(new FileInfo(newName), fullPath.Root));
//            //    }
//            //}
//            //return Json(response);
//        }

//        public JsonResult Get(string target)
//        {
//            FullPath fullPath = ParsePath(target);
//            GetResponse answer = new GetResponse();
//            using (StreamReader reader = new StreamReader(fullPath.File.OpenRead()))
//            {
//                answer.Content = reader.ReadToEnd();
//            }
//            return Json(answer);

//            throw new NotImplementedException();
//            //var id = new Guid(Helper.DecodePath(target));
//            //var file = mediaService.GetMediaFile(id);
//            //if (file == null)
//            //{
//            //    return null;
//            //}
//            //return Json(new MediaFileResult(mediaService, file));
//        }

//        public JsonResult Put(string target, string content)
//        {
//            FullPath fullPath = ParsePath(target);
//            ChangedResponse answer = new ChangedResponse();
//            using (StreamWriter writer = new StreamWriter(fullPath.File.FullName, false))
//            {
//                writer.Write(content);
//            }
//            answer.Changed.Add((FileDTO)DTOBase.Create(fullPath.File, fullPath.Root));
//            return Json(answer);

//            throw new NotSupportedException();
//        }

//        public JsonResult Paste(string source, string dest, IEnumerable<string> targets, bool isCut)
//        {
//            throw new NotImplementedException();

//            //From default
//            //FullPath destPath = ParsePath(dest);
//            //ReplaceResponse response = new ReplaceResponse();
//            //foreach (var item in targets)
//            //{
//            //    FullPath src = ParsePath(item);
//            //    if (src.Directory != null)
//            //    {
//            //        DirectoryInfo newDir = new DirectoryInfo(Path.Combine(destPath.Directory.FullName, src.Directory.Name));
//            //        if (newDir.Exists)
//            //            Directory.Delete(newDir.FullName, true);
//            //        if (isCut)
//            //        {
//            //            RemoveThumbs(src);
//            //            src.Directory.MoveTo(newDir.FullName);
//            //            response.Removed.Add(item);
//            //        }
//            //        else
//            //        {
//            //            DirectoryCopy(src.Directory, newDir.FullName, true);
//            //        }
//            //        response.Added.Add(DTOBase.Create(newDir, destPath.Root));
//            //    }
//            //    else
//            //    {
//            //        string newFilePath = Path.Combine(destPath.Directory.FullName, src.File.Name);
//            //        if (File.Exists(newFilePath))
//            //            File.Delete(newFilePath);
//            //        if (isCut)
//            //        {
//            //            RemoveThumbs(src);
//            //            src.File.MoveTo(newFilePath);
//            //            response.Removed.Add(item);
//            //        }
//            //        else
//            //        {
//            //            File.Copy(src.File.FullName, newFilePath);
//            //        }
//            //        response.Added.Add(DTOBase.Create(new FileInfo(newFilePath), destPath.Root));
//            //    }
//            //}
//            //return Json(response);

//            //OTHER attempt
//            //var destinationId = new Guid(Helper.DecodePath(dest));
//            //var response = new ReplaceResponse();

//            //foreach (var item in targets)
//            //{
//            //    var target = new Guid(Helper.DecodePath(item));

//            //    var folder = mediaService.GetMediaFolder(target);
//            //    if (folder != null)
//            //    {
//            //        folder.ParentId = destinationId;
//            //        mediaService.UpdateMediaFolder(folder);
//            //        response.Removed.Add(item);
//            //    }
//            //    else
//            //    {
//            //        var file = mediaService.GetMediaFile(target);
//            //        if (file != null)
//            //        {
//            //            file.ParentId = destinationId;
//            //            mediaService.UpdateMediaFile(file);
//            //            response.Removed.Add(item);
//            //        }
//            //    }
//            //}
//            //return Json(response);
//        }

//        public JsonResult Upload(string target, HttpFileCollectionBase targets)
//        {
//            var path = Helper.DecodePath(target).Trim(Path.DirectorySeparatorChar);

//            var response = new AddResponse();

//            for (int i = 0; i < targets.AllKeys.Length; i++)
//            {
//                var file = targets[i];
//                if (file != null && file.ContentLength > 0)
//                {
//                    var filePath = storageProvider.Combine(path, file.FileName);
//                    var mediaFile = storageProvider.SaveFile(filePath, file.InputStream);
//                    response.Added.Add(CreateDTO(mediaFile, path));
//                }
//            }
//            return Json(response);
//        }

//        private static DTOBase CreateDTO(IStorageFolder folder, string folderName)
//        {
//            var response = new DirectoryDTO
//            {
//                Mime = "directory",
//                ContainsChildDirs = 1,
//                Hash = Helper.EncodePath(folder.Path),
//                Locked = 0,
//                Read = 1,
//                Write = 1,
//                Size = 0,
//                Name = folder.Name,
//                UnixTimeStamp = (long)(folder.LastUpdated - unixOrigin).TotalSeconds,
//                ParentHash = string.IsNullOrEmpty(folderName) ? Helper.EncodePath("\\") : Helper.EncodePath(folderName)
//            };
//            return response;
//        }

//        private DTOBase CreateDTO(IStorageFile file, string parent)
//        {
//            var hash = Helper.EncodePath(file.GetPath());
//            var response = new FileDTO
//            {
//                Read = 1,
//                Write = 1,
//                Locked = 0,
//                Name = file.GetName(),
//                Size = file.GetSize(),
//                UnixTimeStamp = (long)(file.GetLastUpdated() - unixOrigin).TotalSeconds,
//                Mime = mimeTypeProvider.GetMimeType(file.GetName()),
//                Hash = hash,
//                ParentHash = string.IsNullOrEmpty(parent) ? Helper.EncodePath("\\") : Helper.EncodePath(parent),
//                Url = storageProvider.GetPublicUrl(file.GetPath())
//            };
//            return response;
//        }

//        public JsonResult Thumbs(IEnumerable<string> targets)
//        {
//            ThumbsResponse response = new ThumbsResponse();
//            foreach (string target in targets)
//            {
//                FullPath path = ParsePath(target);
//                response.Images.Add(target, path.Root.GenerateThumbHash(path.File));
//            }
//            return Json(response);
//        }

//        public JsonResult Dim(string target)
//        {
//            FullPath path = ParsePath(target);
//            DimResponse response = new DimResponse(path.Root.GetImageDimension(path.File));
//            return Json(response);
//        }

//        public JsonResult Resize(string target, int width, int height)
//        {
//            FullPath path = ParsePath(target);
//            RemoveThumbs(path);
//            path.Root.PicturesEditor.Resize(path.File.FullName, width, height);
//            var output = new ChangedResponse();
//            output.Changed.Add((FileDTO)DTOBase.Create(path.File, path.Root));
//            return Json(output);
//        }

//        public JsonResult Crop(string target, int x, int y, int width, int height)
//        {
//            FullPath path = ParsePath(target);
//            RemoveThumbs(path);
//            path.Root.PicturesEditor.Crop(path.File.FullName, x, y, width, height);
//            var output = new ChangedResponse();
//            output.Changed.Add((FileDTO)DTOBase.Create(path.File, path.Root));
//            return Json(output);
//        }

//        public JsonResult Rotate(string target, int degree)
//        {
//            FullPath path = ParsePath(target);
//            RemoveThumbs(path);
//            path.Root.PicturesEditor.Rotate(path.File.FullName, degree);
//            var output = new ChangedResponse();
//            output.Changed.Add((FileDTO)DTOBase.Create(path.File, path.Root));
//            return Json(output);
//        }

//        public ActionResult File(string target, bool download)
//        {
//            FullPath fullPath = ParsePath(target);
//            if (fullPath.IsDirectory)
//                return new HttpStatusCodeResult(403, "You can not download whole folder");
//            if (!fullPath.File.Exists)
//                return new HttpNotFoundResult("File not found");
//            if (fullPath.Root.IsShowOnly)
//                return new HttpStatusCodeResult(403, "Access denied. Volume is for show only");
//            return new DownloadFileResult(fullPath.File, download);
//        }

//        public FullPath ParsePath(string target)
//        {
//            throw new NotSupportedException();

//            //var root = _roots.First();
//            //var file = new FileInfo(target);
//            //return new FullPath(root, file);

//            //string volumePrefix = null;
//            //string pathHash = null;
//            //for (int i = 0; i < target.Length; i++)
//            //{
//            //    if (target[i] == '_')
//            //    {
//            //        pathHash = target.Substring(i + 1);
//            //        volumePrefix = target.Substring(0, i + 1);
//            //        break;
//            //    }
//            //}
//            //Root root = _roots.First(r => r.VolumeId == volumePrefix);
//            //string path = Helper.DecodePath(pathHash);
//            //string dirUrl = path != root.Directory.Name ? path : string.Empty;
//            //var dir = new DirectoryInfo(Path.Combine(root.Directory.FullName, dirUrl));
//            //if (dir.Exists)
//            //{
//            //    return new FullPath(root, dir);
//            //}
//            //else
//            //{
//            //    var file = new FileInfo(Path.Combine(root.Directory.FullName, dirUrl));
//            //    return new FullPath(root, file);
//            //}
//        }

//        #region Private

//        private const string _volumePrefix = "v";

//        private void DirectoryCopy(DirectoryInfo sourceDir, string destDirName, bool copySubDirs)
//        {
//            DirectoryInfo[] dirs = sourceDir.GetDirectories();

//            // If the source directory does not exist, throw an exception.
//            if (!sourceDir.Exists)
//            {
//                throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDir.FullName);
//            }

//            // If the destination directory does not exist, create it.
//            if (!Directory.Exists(destDirName))
//            {
//                Directory.CreateDirectory(destDirName);
//            }

//            // Get the file contents of the directory to copy.
//            FileInfo[] files = sourceDir.GetFiles();

//            foreach (FileInfo file in files)
//            {
//                // Create the path to the new copy of the file.
//                string temppath = Path.Combine(destDirName, file.Name);

//                // Copy the file.
//                file.CopyTo(temppath, false);
//            }

//            // If copySubDirs is true, copy the subdirectories.
//            if (copySubDirs)
//            {
//                foreach (DirectoryInfo subdir in dirs)
//                {
//                    // Create the subdirectory.
//                    string temppath = Path.Combine(destDirName, subdir.Name);

//                    // Copy the subdirectories.
//                    DirectoryCopy(subdir, temppath, copySubDirs);
//                }
//            }
//        }

//        private void RemoveThumbs(FullPath path)
//        {
//            //TODO

//            //if (path.Directory != null)
//            //{
//            //    string thumbPath = path.Root.GetExistingThumbPath(path.Directory);
//            //    if (thumbPath != null)
//            //        Directory.Delete(thumbPath, true);
//            //}
//            //else
//            //{
//            //    string thumbPath = path.Root.GetExistingThumbPath(path.File);
//            //    if (thumbPath != null)
//            //        File.Delete(thumbPath);
//            //}
//        }

//        #endregion Private
//    }
//}