using System;

namespace BetterCms.Core.Exceptions.Service
{
    public class SiteLanguageException : CmsException
    {
        public SiteLanguageException(string message) : base(message)
        {
        }

        public SiteLanguageException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
