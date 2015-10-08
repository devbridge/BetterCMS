using System;

namespace BetterCms.Core.Exceptions.Api
{
    /// <summary>
    /// CMS API exception
    /// </summary>
    [Serializable]
    public class CmsApiException : CmsException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CmsApiException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public CmsApiException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CmsApiException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public CmsApiException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
