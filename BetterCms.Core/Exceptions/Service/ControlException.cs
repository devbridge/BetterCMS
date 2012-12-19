using System;

namespace BetterCms.Core.Exceptions.Service
{
    public class ControlException : CmsException
    {
        public ControlException(string message) : base(message)
        {
        }

        public ControlException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
