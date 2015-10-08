using System;

namespace BetterCms.Core.Exceptions.Api
{
    /// <summary>
    /// CMS API validation exception
    /// </summary>
    [Serializable]
    public class CmsApiValidationException : CmsApiException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CmsApiValidationException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public CmsApiValidationException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CmsApiValidationException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public CmsApiValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
