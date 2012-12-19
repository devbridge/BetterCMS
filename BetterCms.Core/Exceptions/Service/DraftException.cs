using System;

namespace BetterCms.Core.Exceptions.Service
{
    public class DraftException : CmsException
    {
        public DraftException(string message) : base(message)
        {
        }

        public DraftException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
