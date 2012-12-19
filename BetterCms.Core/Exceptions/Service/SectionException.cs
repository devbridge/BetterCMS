using System;

namespace BetterCms.Core.Exceptions.Service
{
    public class SectionException : CmsException
    {
        public SectionException(string message) : base(message)
        {
        }

        public SectionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
