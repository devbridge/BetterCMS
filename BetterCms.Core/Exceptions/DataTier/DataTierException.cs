using System;

namespace BetterCms.Core.Exceptions.DataTier
{
    [Serializable]
    public class DataTierException : CmsException
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
