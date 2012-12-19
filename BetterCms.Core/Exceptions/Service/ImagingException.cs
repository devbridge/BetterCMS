using System;

namespace BetterCms.Core.Exceptions.Service
{
    public class ImagingException : CmsException
    {
        public ImagingException(string message) : base(message)
        {
        }

        public ImagingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
