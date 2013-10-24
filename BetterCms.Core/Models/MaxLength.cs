namespace BetterCms.Core.Models
{
    /// <summary>
    /// Defines a maximum length for various field types.
    /// </summary>
    public static class MaxLength
    {
        /// <summary>
        /// A max length of email fields.
        /// </summary>
        public const int Email = 400;

        /// <summary>
        /// A max length of name fields.
        /// </summary>
        public const int Name = 200;

        /// <summary>
        /// A max length of password fields.
        /// </summary>
        public const int Password = 255;

        /// <summary>
        /// A max length of text fields.
        /// </summary>
        public const int Text = 2000;

        /// <summary>
        /// A max length for URL fields.
        /// </summary>
        public const int Url = 850;

        /// <summary>
        /// A max length for URL hash fields.
        /// </summary>
        public const int UrlHash = 32;

        /// <summary>
        /// TA max length for URI fields.
        /// </summary>
        public const int Uri = 2000;

        /// <summary>
        /// The maximum length.
        /// </summary>
        public const int Max = int.MaxValue;
    }
}