using System;

namespace BetterCms.Core.Exceptions.Mvc
{
    /// <summary>
    /// Validation exception with attached resource function.
    /// </summary>
    [Serializable]
    public class ValidationException : CmsException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException" /> class.
        /// </summary>
        /// <param name="resource">A function to retrieve a globalized resource associated with this exception.</param>
        /// <param name="message">The exception message.</param>
        public ValidationException(Func<string> resource, string message)
            : base(message)
        {
            Resource = resource;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException" /> class.
        /// </summary>
        /// <param name="resource">A function to retrieve a globalized resource associated with this exception.</param>
        /// <param name="message">The exception message.</param>
        /// <param name="innerException">The inner exception.</param>
        public ValidationException(Func<string> resource, string message, Exception innerException)
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
