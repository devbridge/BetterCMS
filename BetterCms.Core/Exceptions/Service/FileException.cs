using System;

namespace BetterCms.Core.Exceptions.Service
{
    public class FileException : CmsException
    {
        public FileException(string message) : base(message)
        {
        }

        public FileException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
