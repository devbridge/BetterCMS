using System;

namespace BetterCms.Core.Exceptions.Service
{
    public class SiteException : CmsException
    {
        public SiteException(string message) : base(message)
        {
        }

        public SiteException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
