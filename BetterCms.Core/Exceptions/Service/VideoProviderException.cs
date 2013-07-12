using System;

namespace BetterCms.Core.Exceptions.Service
{
    public class VideoProviderException : CmsException
    {
        public VideoProviderException(string message) : base(message)
        {
        }

        public VideoProviderException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
