using System;

namespace BetterCms.Core.Exceptions.Service
{
    [Serializable]
    public class StorageException : CmsException
    {
        public StorageException(string message) : base(message)
        {
        }

        public StorageException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
