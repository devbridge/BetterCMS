// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InfoViewModel.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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