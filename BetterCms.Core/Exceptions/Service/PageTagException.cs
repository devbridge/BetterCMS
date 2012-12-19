using System;

namespace BetterCms.Core.Exceptions.Service
{
    public class PageTagException : CmsException
    {
        public PageTagException(string message) : base(message)
        {
        }

        public PageTagException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
