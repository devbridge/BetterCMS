// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateFirstUserCommand.cs" company="Devbridge Group LLC">
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

using BetterCms.Core.Exceptions.Mvc;

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Users.Models;
using BetterCms.Module.Users.Services;
using BetterCms.Module.Users.ViewModels.Registration;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Users.Commands.Registration
{
    public class CreateFirstUserCommand : CommandBase, ICommandIn<CreateFirstUserViewModel>
    {
        private readonly IAuthenticationService authenticationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateFirstUserCommand" /> class.
        /// </summary>
        /// <param name="authenticationService">The authentication service.</param>
        public CreateFirstUserCommand(IAuthenticationService authenticationService)
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
        public void Execute(CreateFirstUserViewModel request)
        {
            if (Repository.AsQueryable<Models.User>().Any())
            {
                throw new ValidationException(() => "First user already exists.", string.Format("Failed to create first user from the model {0}.", request));
            }

            var user = new Models.User();
            var salt = authenticationService.GeneratePasswordSalt();
            var systemRoles = Repository.AsQueryable<Models.Role>().Where(f => f.IsSystematic).ToList();

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.UserName = request.UserName;
            user.Password = authenticationService.CreatePasswordHash(request.Password, salt);
            user.Salt = salt;
            user.UserRoles = systemRoles.Select(role => new UserRole
                                                         {
                                                             User = user,
                                                             Role = role
                                                         })
                                        .ToList();
            
            Repository.Save(user);
            UnitOfWork.Commit();

            // Notify
            Events.UserEvents.Instance.OnUserCreated(user);
        }
    }
}