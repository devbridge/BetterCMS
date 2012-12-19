using System;

namespace BetterCms.Core.Exceptions.Service
{
    public class RegionException : CmsException
    {
        public RegionException(string message) : base(message)
        {
        }

        public RegionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
