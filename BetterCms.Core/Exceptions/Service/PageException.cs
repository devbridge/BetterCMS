using System;

namespace BetterCms.Core.Exceptions.Service
{
    [Serializable]
    public class PageException : CmsException
    {
        public PageException(string message) : base(message)
        {
        }

        public PageException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
