// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegistrationController.cs" company="Devbridge Group LLC">
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
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Users.Commands.Registration;
using BetterCms.Module.Users.ViewModels.Registration;

using Microsoft.Web.Mvc;

namespace BetterCms.Module.Users.Controllers
{
    /// <summary>
    /// User management.
    /// </summary>    
    [ActionLinkArea(UsersModuleDescriptor.UsersAreaName)]
    public class RegistrationController : CmsControllerBase
    {
        private readonly UsersModuleDescriptor usersModuleDescriptor;

        public RegistrationController(UsersModuleDescriptor usersModuleDescriptor)
        {
            this.usersModuleDescriptor = usersModuleDescriptor;
        }

        /// <summary>
        /// Creates the first user.
        /// </summary>        
        [HttpGet]
        public ActionResult CreateFirstUser()
        {
            if (usersModuleDescriptor.IsFirstUserRegistered)
            {
                throw new HttpException(403, "First user is already registered!");
            }

            return View(new CreateFirstUserViewModel());
        }

        /// <summary>
        /// Creates the first user.
        /// </summary>
        /// <param name="model">The model.</param>
        [HttpPost]
        public ActionResult CreateFirstUser(CreateFirstUserViewModel model)
        {
            if (usersModuleDescriptor.IsFirstUserRegistered)
            {
                throw new HttpException(403, "First user is already registered!");
            }

            if (ModelState.IsValid)
            {
                if (GetCommand<CreateFirstUserCommand>().ExecuteCommand(model))
                {
                    usersModuleDescriptor.SetAsFirstUserRegistered();

                    return Redirect(FormsAuthentication.LoginUrl ?? "/");
                }
            }

            return View(model);
        }
    }
}
