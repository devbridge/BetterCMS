using System;
using System.Web.Mvc;

namespace BetterCms.Module.Users.ViewModels.Role
{
    public class PermissionViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public Guid Id { get; set; }

        [HiddenInput(DisplayValue = true)]
        public string Name { get; set; }

        public bool IsSelected { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Id: {0}, Name: {1}, IsSelected: {2}", Id, Name, IsSelected);
        }
    }
}