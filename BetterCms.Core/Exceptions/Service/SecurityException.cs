using System;

namespace BetterCms.Core.Exceptions.Service
{
    public class SecurityException : CmsException
    {
        public SecurityException(string message) : base(message)
        {
        }

        public SecurityException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
