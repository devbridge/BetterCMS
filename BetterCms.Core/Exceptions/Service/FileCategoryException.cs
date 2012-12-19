using System;

namespace BetterCms.Core.Exceptions.Service
{
    public class FileCategoryException : CmsException
    {
        public FileCategoryException(string message) : base(message)
        {
        }

        public FileCategoryException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
