using System;

namespace BetterCms.Core.Exceptions.Mvc
{
    [Serializable]
    public class ConfirmationRequestException : CmsException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfirmationRequestException" /> class.
        /// </summary>
        /// <param name="resource">A function to retrieve a globalized resource associated with this exception.</param>
        /// <param name="message">The exception message.</param>
        public ConfirmationRequestException(Func<string> resource, string message)
            : base(message)
        {
            Resource = resource;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfirmationRequestException" /> class.
        /// </summary>
        /// <param name="resource">A function to retrieve a globalized resource associated with this exception.</param>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public ConfirmationRequestException(Func<string> resource, string message, Exception innerException)
            : base(message, innerException)
        {
            Resource = resource;
        }

        /// <summary>
        /// Gets a function to retrieve a globalized resource associated with this exception.
        /// </summary>
        /// <value>
        /// A function to retrieve a globalized resource associated with this exception.
        /// </value>
        public Func<string> Resource { get; private set; }
    }
}
