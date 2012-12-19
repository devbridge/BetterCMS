using System;

namespace BetterCms.Core.Exceptions.Service
{
    public class RedirectException : CmsException
    {
        public RedirectException(string message) : base(message)
        {
        }

        public RedirectException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
