using System;

namespace BetterCms.Core.Exceptions.Service
{
    public class AuthorException : CmsException
    {
        public AuthorException(string message) : base(message)
        {
        }

        public AuthorException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
