using System;

namespace BetterCms.Core.Exceptions.Service
{
    public class PageHtmlControlException : CmsException
    {
        public PageHtmlControlException(string message) : base(message)
        {
        }

        public PageHtmlControlException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
