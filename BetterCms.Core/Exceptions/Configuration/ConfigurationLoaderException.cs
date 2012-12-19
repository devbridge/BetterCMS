using System;

namespace BetterCms.Core.Exceptions.Configuration
{
    public class ConfigurationLoaderException : CmsException
    {
        public ConfigurationLoaderException(string message) : base(message)
        {
        }

        public ConfigurationLoaderException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
