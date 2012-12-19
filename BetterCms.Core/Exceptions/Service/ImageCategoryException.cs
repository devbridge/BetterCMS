using System;

namespace BetterCms.Core.Exceptions.Service
{
    public class ImageCategoryException : CmsException
    {
        public ImageCategoryException(string message) : base(message)
        {
        }

        public ImageCategoryException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
