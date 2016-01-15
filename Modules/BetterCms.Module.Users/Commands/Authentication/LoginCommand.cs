// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoginCommand.cs" company="Devbridge Group LLC">
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
using System;
using System.Web;
using System.Web.Security;

using BetterCms.Core.Exceptions;
using BetterCms.Core.Exceptions.Mvc;

using BetterCms.Module.Root.Mvc;

using BetterCms.Module.Users.Content.Resources;
using BetterCms.Module.Users.Services;
using BetterCms.Module.Users.ViewModels.Authentication;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Users.Commands.Authentication
{
    public class LoginCommand : CommandBase, ICommand<LoginViewModel, HttpCookie>
    {
        private readonly IAuthenticationService authenticationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginCommand" /> class.
        /// </summary>
        /// <param name="authenticationService">The authentication service.</param>
        public LoginCommand(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }

        /// <summary>
        /// Executes a command to save role.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// Response with id and version
        /// </returns>
        public HttpCookie Execute(LoginViewModel request)
        {
            if (Membership.ValidateUser(request.UserName, request.Password))
            {
                if (!Roles.Enabled)
                {
                    throw new CmsException("A roles provider should be enabled in web.config.");
                }

                var authTicket = new FormsAuthenticationTicket(1, request.UserName, DateTime.Now, DateTime.Now.AddMonths(1), request.RememberMe, string.Empty);

                var cookieContents = FormsAuthentication.Encrypt(authTicket);
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieContents) {
                                                                                                     Expires = authTicket.Expiration,
                                                                                                     Path = FormsAuthentication.FormsCookiePath
                                                                                                 };
                return cookie;
            }

            throw new ValidationException(() => UsersGlobalization.Login_UserNameOrPassword_Invalid, "User name or password is invalid.");
        }
    }
}