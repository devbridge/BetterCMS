using System;

namespace BetterCms.Core.Exceptions.Service
{
    public class TagException : CmsException
    {
        public TagException(string message) : base(message)
        {
        }

        public TagException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
