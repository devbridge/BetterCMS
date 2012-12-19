using System;
using System.Collections.Specialized;
using System.IO;

namespace BetterCms.Core.Services.Storage
{
    [Serializable]
    public class UploadRequest
    {
        public Stream InputStream { get; set; }

        public Uri Uri { get; set; }

        public NameValueCollection MetaData { get; set; }

        public NameValueCollection Headers { get; set; }

        public bool CreateDirectory { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, Stream Length: {1}, MetaData: {2}, CreateDirectory: {3}", Uri, InputStream.Length, MetaData, CreateDirectory);
        }
    }
}
