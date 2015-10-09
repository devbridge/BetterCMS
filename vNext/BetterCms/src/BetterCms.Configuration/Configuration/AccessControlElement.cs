namespace BetterCms.Configuration
{
    public class AccessControlElement
    {

        /// <summary>
        /// Gets or sets the role or user.
        /// </summary>
        /// <value>
        /// The role or user.
        /// </value>
        public string Identity { get; set; }

        /// <summary>
        /// Gets or sets the access level.
        /// </summary>
        /// <value>
        /// The access level.
        /// </value>
        public string AccessLevel { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether this access rule is role based.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this access rule is role bases; otherwise, <c>false</c>.
        /// </value>
        public bool IsRole { get; set; }

    }
}