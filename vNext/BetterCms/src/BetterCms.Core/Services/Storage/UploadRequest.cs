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

        /// <summary>
        /// Gets or sets a value indicating whether to ignore access control (e.g. Access Control is ignored for images).
        /// </summary>
        /// <value>
        ///   <c>true</c> if ignore access control; otherwise, <c>false</c>.
        /// </value>
        public bool IgnoreAccessControl { get; set; }

        public override string ToString()
        {
            return
                $"{Uri}, Stream Length: {InputStream.Length}, MetaData: {MetaData}, CreateDirectory: {CreateDirectory}";
        }
    }
}
