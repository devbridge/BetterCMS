// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultRegistrationService.cs" company="Devbridge Group LLC">
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
using System.Linq;

using BetterCms.Module.Users.Models;

using BetterModules.Core.DataAccess;
using BetterModules.Core.Web.Web;

namespace BetterCms.Module.Users.Services
{
    public class DefaultRegistrationService : IRegistrationService
    {
        private readonly IRepository repository;

        private readonly IHttpContextAccessor httpContextAccessor;

        public DefaultRegistrationService(IRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.repository = repository;
        }

        public bool IsFirstUserRegistered()
        {
            return repository.AsQueryable<User>().Any();
        }

        public void NavigateToRegisterFirstUserPage()
        {
            const string url = "/" + UsersModuleDescriptor.UsersAreaName + "/register";

            var http = httpContextAccessor.GetCurrent();

            if (http != null && http.Request != null && http.Request.Url != null)
            {
                if (http.Request.Url.AbsolutePath.EndsWith(".css", System.StringComparison.Ordinal))
                {
                    return;
                }
                if (!http.Request.Url.PathAndQuery.Contains(url))
                {
                    http.Response.Redirect(url, true);
                }
            }
        }
    }
}
