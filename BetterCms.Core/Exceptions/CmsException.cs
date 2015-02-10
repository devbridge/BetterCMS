using System;

using Devbridge.Platform.Core.Exceptions;

namespace BetterCms.Core.Exceptions
{
    /// <summary>
    /// Generic CMS exception.
    /// </summary>
    [Serializable]
    public class CmsException : PlatformException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CmsException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public CmsException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CmsException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public CmsException(string message, Exception innerException) : base(message, innerException)
        {
        }        
    }
}
