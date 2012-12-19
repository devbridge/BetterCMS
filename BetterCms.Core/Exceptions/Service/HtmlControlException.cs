using System;

namespace BetterCms.Core.Exceptions.Service
{
    public class HtmlControlException : CmsException
    {
        public HtmlControlException(string message) : base(message)
        {
        }

        public HtmlControlException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
