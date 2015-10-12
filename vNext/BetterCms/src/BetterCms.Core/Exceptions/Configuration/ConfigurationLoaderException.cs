using System;

namespace BetterCms.Core.Exceptions.Configuration
{
    //TODO check if this is still relevant
    [Serializable]
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
