// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultUserProfileUrlResolver.cs" company="Devbridge Group LLC">
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
using System.Web.Security;

using BetterCms.Module.Root.Services;
using BetterCms.Module.Users.Controllers;
using BetterCms.Module.Users.Provider;

using BetterModules.Core.Web.Web;

namespace BetterCms.Module.Users.Services
{
    public class DefaultUserProfileUrlResolver : IUserProfileUrlResolver
    {
        /// <summary>
        /// The context accessor
        /// </summary>
        private readonly IHttpContextAccessor contextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultUserProfileUrlResolver" /> class.
        /// </summary>
        /// <param name="contextAccessor">The context accessor.</param>
        public DefaultUserProfileUrlResolver(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        /// <summary>
        /// Gets the edit user profile URL.
        /// </summary>
        public string GetEditUserProfileUrl()
        {
            if (!Roles.Enabled || !(Roles.Provider is CmsRoleProvider) || !(Membership.Provider is CmsMembershipProvider))
            {
                return null;
            }

            return contextAccessor.ResolveActionUrl<UserProfileController>(f => f.EditProfile());
        }
    }
}