using System;

namespace BetterCms.Core.Exceptions.Service
{
    public class UserException : CmsException
    {
        public UserException(string message) : base(message)
        {
        }

        public UserException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
