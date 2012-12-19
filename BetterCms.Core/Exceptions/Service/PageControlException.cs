using System;

namespace BetterCms.Core.Exceptions.Service
{
    public class PageControlException : CmsException
    {
        public PageControlException(string message) : base(message)
        {
        }

        public PageControlException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
