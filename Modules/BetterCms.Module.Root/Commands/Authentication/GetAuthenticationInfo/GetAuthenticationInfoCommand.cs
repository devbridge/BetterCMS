// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetAuthenticationInfoCommand.cs" company="Devbridge Group LLC">
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

using BetterCms.Module.Root.Models.Authentication;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Root.Commands.Authentication.GetAuthenticationInfo
{
    public class GetAuthenticationInfoCommand : CommandBase, ICommandOut<InfoViewModel>
    {
        /// <summary>
        /// The URL resolver
        /// </summary>
        private readonly IUserProfileUrlResolver urlResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAuthenticationInfoCommand" /> class.
        /// </summary>
        /// <param name="urlResolver">The URL resolver.</param>
        public GetAuthenticationInfoCommand(IUserProfileUrlResolver urlResolver)
        {
            this.urlResolver = urlResolver;
        }

        public InfoViewModel Execute()
        {
            var model = new InfoViewModel();
            model.IsUserAuthenticated = Context.Principal.Identity.IsAuthenticated;
            model.UserName = Context.Principal.Identity.Name;
            model.EditUserProfileUrl = urlResolver.GetEditUserProfileUrl();

            return model;
        }
    }
}