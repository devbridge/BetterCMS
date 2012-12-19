using System;

namespace BetterCms.Core.Exceptions.Service
{
    public class LayoutException : CmsException
    {
        public LayoutException(string message) : base(message)
        {
        }

        public LayoutException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
