using System;

namespace Devbridge.Platform.Core.Exceptions
{
    /// <summary>
    /// Generic Platform exception.
    /// </summary>
    [Serializable]
    public class PlatformException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlatformException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public PlatformException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlatformException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public PlatformException(string message, Exception innerException) : base(message, innerException)
        {
        }        
    }
}
