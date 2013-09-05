using System.ComponentModel.DataAnnotations;

using BetterCms.Module.Root.ViewModels.Cms;

namespace BetterCms.Module.Users.ViewModels
{
    public class LoginViewModel : RenderWidgetViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

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