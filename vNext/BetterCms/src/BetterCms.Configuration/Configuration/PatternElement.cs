namespace BetterCms.Configuration
{
    public class PatternElement
    {
        /// <summary>
        /// Gets or sets the expression.
        /// </summary>
        /// <value>
        /// The expression.
        /// </value>
        public string Expression { get; set; } = "";

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="PatternElement"/> should be negated.
        /// </summary>
        /// <value>
        ///   <c>true</c> if negate; otherwise, <c>false</c>.
        /// </value>
        public bool Negate { get; set; } = false;

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; } = "";

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="PatternElement"/> should be checked by ignoring case.
        /// </summary>
        /// <value>
        ///   <c>true</c> if ignore case; otherwise, <c>false</c>.
        /// </value>
        public bool IgnoreCase { get; set; } = false;
    }
}