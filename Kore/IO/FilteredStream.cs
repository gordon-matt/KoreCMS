//using System;
//using System.IO;
//using System.Text;

//namespace Kore.IO
//{
//    // Thanks to: Arran Maclean (https://arranmaclean.wordpress.com/2010/08/10/minify-html-with-net-mvc-actionfilter/)
//    public class FilteredStream : Stream
//    {
//        private Func<string, string> filter;
//        private Stream stream;

//        public FilteredStream(Stream stream, Func<string, string> filter)
//        {
//            this.stream = stream;
//            this.filter = filter;
//        }

//        public override bool CanRead { get { return true; } }

//        public override bool CanSeek { get { return true; } }

//        public override bool CanWrite { get { return true; } }

//        public override long Length { get { return 0; } }

//        public override long Position { get; set; }

//        public override void Close()
//        {
//            stream.Close();
//        }

//        public override void Flush()
//        {
//            stream.Flush();
//        }

//        public override int Read(byte[] buffer, int offset, int count)
//        {
//            return stream.Read(buffer, offset, count);
//        }

//        public override long Seek(long offset, SeekOrigin origin)
//        {
//            return stream.Seek(offset, origin);
//        }

//        public override void SetLength(long value)
//        {
//            stream.SetLength(value);
//        }

//        public override void Write(byte[] buffer, int offset, int count)
//        {
//            // capture the data and convert to string
//            byte[] data = new byte[count];
//            Buffer.BlockCopy(buffer, offset, data, 0, count);
//            string s = Encoding.Default.GetString(buffer);

//            // filter the string
//            s = filter(s);

//            // write the data to stream
//            byte[] outdata = Encoding.Default.GetBytes(s);
//            stream.Write(outdata, 0, outdata.GetLength(0));
//        }
//    }
//}