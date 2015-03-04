using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ElFinder
{
    internal static class Helper
    {
        public static string DecodePath(string path)
        {
            return UTF8Encoding.UTF8.GetString(HttpServerUtility.UrlTokenDecode(path));
        }

        public static string EncodePath(string path)
        {
            return HttpServerUtility.UrlTokenEncode(UTF8Encoding.UTF8.GetBytes(path));
        }

        public static string GetDuplicatedName(FileInfo file)
        {
            var parentPath = file.DirectoryName;
            var name = Path.GetFileNameWithoutExtension(file.Name);
            var ext = file.Extension;

            var newName = string.Format(@"{0}\{1} copy{2}", parentPath, name, ext);
            if (!File.Exists(newName))
            {
                return newName;
            }
            else
            {
                bool finded = false;
                for (int i = 1; i < 10 && !finded; i++)
                {
                    newName = string.Format(@"{0}\{1} copy {2}{3}", parentPath, name, i, ext);
                    if (!File.Exists(newName))
                        finded = true;
                }
                if (!finded)
                    newName = string.Format(@"{0}\{1} copy {2}{3}", parentPath, name, Guid.NewGuid(), ext);
            }

            return newName;
        }

        public static string GetFileMd5(FileInfo info)
        {
            return GetFileMd5(info.Name, info.LastWriteTimeUtc);
        }

        public static string GetFileMd5(string fileName, DateTime modified)
        {
            var encoder = Encoding.UTF8.GetEncoder();

            fileName += modified.ToFileTimeUtc();
            char[] fileNameChars = fileName.ToCharArray();
            byte[] buffer = new byte[encoder.GetByteCount(fileNameChars, 0, fileName.Length, true)];
            encoder.GetBytes(fileNameChars, 0, fileName.Length, buffer, 0, true);

            var md5 = new MD5CryptoServiceProvider();
            return BitConverter.ToString(md5.ComputeHash(buffer)).Replace("-", string.Empty);
        }

        public static string GetMimeType(FileInfo file)
        {
            if (file.Extension.Length > 1)
            {
                return Mime.GetMimeType(file.Extension.ToLower().Substring(1));
            }
            else
            {
                return "unknown";
            }
        }

        public static string GetMimeType(string ext)
        {
            return Mime.GetMimeType(ext);
        }
    }
}