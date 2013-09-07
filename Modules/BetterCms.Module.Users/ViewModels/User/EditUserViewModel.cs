using System.Collections.Generic;

namespace BetterCms.Module.Users.ViewModels.User
{
    public class EditUserViewModel : EditUserProfileViewModel
    {
        /// <summary>
        /// Gets or sets the list of the roles.
        /// </summary>
        /// <value>
        /// The list of the roles.
        /// </value>
        public IList<string> Roles { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}", base.ToString());
        }
    }
}