using System;

namespace BetterCms.Core.Exceptions.Service
{
    public class RolesException : CmsException
    {
        public RolesException(string message) : base(message)
        {
        }

        public RolesException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
