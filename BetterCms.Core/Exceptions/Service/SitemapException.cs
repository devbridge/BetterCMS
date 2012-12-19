using System;

namespace BetterCms.Core.Exceptions.Service
{
    public class SitemapException : CmsException
    {
        public SitemapException(string message) : base(message)
        {
        }

        public SitemapException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
