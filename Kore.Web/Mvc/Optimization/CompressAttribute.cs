using System;
using System.IO;
using System.IO.Compression;
using System.Web;
using System.Web.Mvc;

namespace Kore.Web.Mvc.Optimization
{
    public class CompressAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            string acceptEncoding = filterContext.HttpContext.Request.Headers["Accept-Encoding"];
            if (!string.IsNullOrEmpty(acceptEncoding))
            {
                acceptEncoding = acceptEncoding.ToUpperInvariant();
                HttpResponseBase response = filterContext.HttpContext.Response;

                if (response.Filter == null)
                {
                    return;
                }

                if (acceptEncoding.Contains("GZIP"))
                {
                    response.AppendHeader("Content-encoding", "gzip");
                    //response.Filter = new GZipStream(response.Filter, CompressionMode.Compress);
                    response.Filter = new CompressionFilter(response.Filter, CompressionType.GZip);
                }
                else if (acceptEncoding.Contains("DEFLATE"))
                {
                    response.AppendHeader("Content-encoding", "deflate");
                    //response.Filter = new DeflateStream(response.Filter, CompressionMode.Compress);
                    response.Filter = new CompressionFilter(response.Filter, CompressionType.Deflate);
                }
            }
        }
    }

    public enum CompressionType
    {
        Deflate,
        GZip
    }

    /// <summary>
    /// Thanks to: Jon Hanna (http://stackoverflow.com/questions/3456773/firefox-issues-with-compression-filter-attribute-in-asp-net-mvc)
    /// </summary>
    public sealed class CompressionFilter : Stream
    {
        private readonly Stream compSink;
        private readonly Stream finalSink;

        public CompressionFilter(Stream stream, CompressionType compressionType)
        {
            switch (compressionType)
            {
                case CompressionType.Deflate:
                    compSink = new DeflateStream((finalSink = stream), CompressionMode.Compress);
                    break;

                case CompressionType.GZip:
                    compSink = new GZipStream((finalSink = stream), CompressionMode.Compress);
                    break;

                default:
                    throw new ArgumentException();
            }
        }

        public Stream Sink
        {
            get { return finalSink; }
        }

        public CompressionType CompressionType
        {
            get { return compSink is DeflateStream ? CompressionType.Deflate : CompressionType.GZip; }
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override long Length
        {
            get { throw new NotSupportedException(); }
        }

        public override long Position
        {
            get { throw new NotSupportedException(); }
            set { throw new NotSupportedException(); }
        }

        public override void Flush()
        {
            //We do not flush the compression stream. At best this does nothing, at worse it
            //loses a few bytes. We do however flush the underlying stream to send bytes down the
            //wire.
            finalSink.Flush();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            compSink.Write(buffer, offset, count);
        }

        public override void WriteByte(byte value)
        {
            compSink.WriteByte(value);
        }

        public override void Close()
        {
            compSink.Close();
            finalSink.Close();
            base.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                compSink.Dispose();
                finalSink.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}