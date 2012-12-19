using System;

namespace BetterCms.Core.Exceptions.Service
{
    public class ContentException : CmsException
    {
        public ContentException(string message) : base(message)
        {
        }

        public ContentException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
