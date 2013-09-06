namespace BetterCms.Module.Root.Models.Authentication
{
    /// <summary>
    /// View model of sidebar user partial view.
    /// </summary>
    public class InfoViewModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether user is authenticated.
        /// </summary>
        /// <value>
        /// <c>true</c> if user is authenticated; otherwise, <c>false</c>.
        /// </value>
        public bool IsUserAuthenticated { get; set; }

        /// <summary>
        /// Gets or sets the name of authenticated user.
        /// </summary>
        /// <value>
        /// The name of authenticated user.
        /// </value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the edit user profile URL.
        /// </summary>
        /// <value>
        /// The edit user profile URL.
        /// </value>
        public string EditUserProfileUrl { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("UserName: {0}", UserName);
        }
    }
}