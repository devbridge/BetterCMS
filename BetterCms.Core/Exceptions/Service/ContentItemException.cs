using System;

namespace BetterCms.Core.Exceptions.Service
{
    public class ContentItemException : CmsException
    {
        public ContentItemException(string message) : base(message)
        {
        }

        public ContentItemException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
