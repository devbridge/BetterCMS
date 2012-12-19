using System;

namespace BetterCms.Core.Exceptions.Service
{
    public class PageException : CmsException
    {
        public PageException(string message) : base(message)
        {
        }

        public PageException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
