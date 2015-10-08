using System;
using System.IO;

namespace BetterCms.Core.Services.Storage
{
    [Serializable]
    public class DownloadResponse
    {
        public Uri Uri { get; set; }

        public Stream ResponseStream { get; set; }
    }
}
