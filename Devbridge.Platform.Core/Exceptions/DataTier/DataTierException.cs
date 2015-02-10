using System;

namespace Devbridge.Platform.Core.Exceptions.DataTier
{
    [Serializable]
    public class DataTierException : PlatformException
    {
        public DataTierException(string message)
            : base(message)
        {
        }

        public DataTierException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
