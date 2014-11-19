using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ElFinder;
using ElFinder.DTO;
using ElFinder.Response;
using Kore.Web.ContentManagement.FileSystems.Media;
using Kore.Web.Mvc;

namespace Kore.Web.ContentManagement.Areas.Admin.Media.Services
{
    public class SystemMediaDriver : IDriver
    {
        private readonly IStorageProvider storageProvider;
        private readonly IMimeTypeProvider mimeTypeProvider;
        private static readonly DateTime unixOrigin = new DateTime(1970, 1, 1, 0, 0, 0);

        public SystemMediaDriver(IStorageProvider storageProvider, IMimeTypeProvider mimeTypeProvider)
        {
            this.storageProvider = storageProvider;
            this.mimeTypeProvider = mimeTypeProvider;
        }

        private static JsonResult Json(object data)
        {
            return new JsonNetResult(data);
        }

        public JsonResult Open(string target, bool tree)
        {
            if (string.IsNullOrEmpty(target))
            {
                return Init(target);
            }

            var path = Helper.DecodePath(target).Trim(Path.DirectorySeparatorChar);

            if (string.IsNullOrEmpty(path))
            {
                return Init(target);
            }

            var folder = storageProvider.GetFolder(path);
            var folders = storageProvider.ListFolders(path);
            var files = storageProvider.ListFiles(path);

            var response = new OpenResponse(CreateDTO(folder, Path.GetDirectoryName(path)));

            foreach (var file in files)
            {
                response.AddResponse(CreateDTO(file, path));
            }

            foreach (var child in folders)
            {
                response.AddResponse(CreateDTO(child, path));
            }
            return Json(response);
        }

        public JsonResult Init(string target)
        {
            var folders = storageProvider.ListFolders(null);
            var files = storageProvider.ListFiles(null);

            var directory = new RootDTO
            {
                Mime = "directory",
                Dirs = folders.Any() ? (byte)1 : (byte)0,
                Hash = Helper.EncodePath("\\"),
                Locked = 0,
                Name = "Media",
                Read = 1,
                Write = 1,
                Size = 0,
                UnixTimeStamp = (long)(DateTime.UtcNow - unixOrigin).TotalSeconds,
                VolumeId = "v1"
            };

            var response = new InitResponse(directory);

            foreach (var file in files)
            {
                response.AddResponse(CreateDTO(file, null));
            }

            foreach (var folder in folders)
            {
                response.AddResponse(CreateDTO(folder, null));
            }

            response.Options.Path = "Media";
            response.Options.Url = "/";
            response.Options.ThumbnailsUrl = "/";

            return Json(response);
        }

        public JsonResult Parents(string target)
        {
            //var id = new Guid(Helper.DecodePath(target));
            //var folders = mediaService.GetMediaFolders(id);
            //var answer = new TreeResponse();
            //return Json(answer);
            throw new NotSupportedException();
        }

        public JsonResult Tree(string target)
        {
            var path = Helper.DecodePath(target).Trim(Path.DirectorySeparatorChar);
            var parentPath = string.IsNullOrEmpty(path) ? null : Path.GetDirectoryName(path);
            var folders = storageProvider.ListFolders(path);
            var answer = new TreeResponse();
            foreach (var folder in folders)
            {
                answer.Tree.Add(CreateDTO(folder, parentPath));
            }
            return Json(answer);
        }

        public JsonResult List(string target)
        {
            var path = Helper.DecodePath(target);
            var files = storageProvider.ListFiles(path);
            var answer = new ListResponse();

            foreach (var file in files)
            {
                answer.List.Add(file.GetName());
            }

            return Json(answer);
        }

        public JsonResult MakeDir(string target, string name)
        {
            var path = Helper.DecodePath(target).Trim(Path.DirectorySeparatorChar);
            var folder = storageProvider.CreateFolder(storageProvider.Combine(path, name));
            return Json(new AddResponse(CreateDTO(folder, path)));
        }

        public JsonResult MakeFile(string target, string name)
        {
            throw new NotSupportedException();
        }

        public JsonResult Rename(string target, string name)
        {
            var path = Helper.DecodePath(target);
            var folderPath = Path.GetDirectoryName(path);
            var answer = new ReplaceResponse();

            if (storageProvider.FileExists(path))
            {
                var oldFileName = Path.GetFileName(path);
                var mediaFile = storageProvider.RenameFile(storageProvider.Combine(folderPath, oldFileName), storageProvider.Combine(folderPath, name));
                answer.Removed.Add(target);
                answer.Added.Add(CreateDTO(mediaFile, path));
            }
            else
            {
                var mediaFolder = storageProvider.GetFolder(path);
                if (mediaFolder != null)
                {
                    mediaFolder = storageProvider.RenameFolder(path, name);

                    answer.Removed.Add(target);
                    answer.Added.Add(CreateDTO(mediaFolder, folderPath));
                }
            }

            return Json(answer);
        }

        public JsonResult Remove(IEnumerable<string> targets)
        {
            var answer = new RemoveResponse();
            foreach (var item in targets)
            {
                var path = Helper.DecodePath(item);

                if (storageProvider.FileExists(path))
                {
                    storageProvider.DeleteFile(path);
                }
                else if (storageProvider.FolderExists(path))
                {
                    storageProvider.DeleteFolder(path);
                }

                answer.Removed.Add(item);
            }
            return Json(answer);
        }

        public JsonResult Duplicate(IEnumerable<string> targets)
        {
            throw new NotSupportedException();
        }

        public JsonResult Get(string target)
        {
            throw new NotImplementedException();
            //var id = new Guid(Helper.DecodePath(target));
            //var file = mediaService.GetMediaFile(id);
            //if (file == null)
            //{
            //    return null;
            //}
            //return Json(new MediaFileResult(mediaService, file));
        }

        public JsonResult Put(string target, string content)
        {
            throw new NotSupportedException();
        }

        public JsonResult Paste(string source, string dest, IEnumerable<string> targets, bool isCut)
        {
            throw new NotImplementedException();
            //var destinationId = new Guid(Helper.DecodePath(dest));
            //var response = new ReplaceResponse();

            //foreach (var item in targets)
            //{
            //    var target = new Guid(Helper.DecodePath(item));

            //    var folder = mediaService.GetMediaFolder(target);
            //    if (folder != null)
            //    {
            //        folder.ParentId = destinationId;
            //        mediaService.UpdateMediaFolder(folder);
            //        response.Removed.Add(item);
            //    }
            //    else
            //    {
            //        var file = mediaService.GetMediaFile(target);
            //        if (file != null)
            //        {
            //            file.ParentId = destinationId;
            //            mediaService.UpdateMediaFile(file);
            //            response.Removed.Add(item);
            //        }
            //    }
            //}
            //return Json(response);
        }

        public JsonResult Upload(string target, HttpFileCollectionBase targets)
        {
            var path = Helper.DecodePath(target).Trim(Path.DirectorySeparatorChar);

            var response = new AddResponse();

            for (int i = 0; i < targets.AllKeys.Length; i++)
            {
                var file = targets[i];
                if (file != null && file.ContentLength > 0)
                {
                    var filePath = storageProvider.Combine(path, file.FileName);
                    var mediaFile = storageProvider.SaveFile(filePath, file.InputStream);
                    response.Added.Add(CreateDTO(mediaFile, path));
                }
            }
            return Json(response);
        }

        private static DTOBase CreateDTO(IStorageFolder folder, string folderName)
        {
            var response = new DirectoryDTO
            {
                Mime = "directory",
                ContainsChildDirs = 1,
                Hash = Helper.EncodePath(folder.GetPath()),
                Locked = 0,
                Read = 1,
                Write = 1,
                Size = 0,
                Name = folder.GetName(),
                UnixTimeStamp = (long)(folder.GetLastUpdated() - unixOrigin).TotalSeconds,
                ParentHash = string.IsNullOrEmpty(folderName) ? Helper.EncodePath("\\") : Helper.EncodePath(folderName)
            };
            return response;
        }

        private DTOBase CreateDTO(IStorageFile file, string parent)
        {
            var hash = Helper.EncodePath(file.GetPath());
            var response = new FileDTO
            {
                Read = 1,
                Write = 1,
                Locked = 0,
                Name = file.GetName(),
                Size = file.GetSize(),
                UnixTimeStamp = (long)(file.GetLastUpdated() - unixOrigin).TotalSeconds,
                Mime = mimeTypeProvider.GetMimeType(file.GetName()),
                Hash = hash,
                ParentHash = string.IsNullOrEmpty(parent) ? Helper.EncodePath("\\") : Helper.EncodePath(parent),
                Url = storageProvider.GetPublicUrl(file.GetPath())
            };
            return response;
        }

        public JsonResult Thumbs(IEnumerable<string> targets)
        {
            throw new NotImplementedException();
        }

        public JsonResult Dim(string target)
        {
            throw new NotImplementedException();
        }

        public JsonResult Resize(string target, int width, int height)
        {
            throw new NotImplementedException();
        }

        public JsonResult Crop(string target, int x, int y, int width, int height)
        {
            throw new NotImplementedException();
        }

        public JsonResult Rotate(string target, int degree)
        {
            throw new NotImplementedException();
        }

        public ActionResult File(string target, bool download)
        {
            throw new NotImplementedException();
        }

        public FullPath ParsePath(string target)
        {
            throw new NotImplementedException();
        }
    }
}