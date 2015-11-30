// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandBase.cs" company="Devbridge Group LLC">
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
using System.Collections.Generic;

using BetterCms.Core.Security;
using BetterCms.Core.Services;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.ViewModels.Security;

namespace BetterCms.Module.Root.Mvc
{
    /// <summary>
    /// A base class for commands.
    /// </summary>
    public abstract class CommandBase : BetterModules.Core.Web.Mvc.Commands.CoreCommandBase
    {
        /// <summary>
        /// Gets or sets the security service.
        /// </summary>
        /// <value>
        /// The security service.
        /// </value>
        public virtual ISecurityService SecurityService { get; set; }

        /// <summary>
        /// Gets or sets the access control service.
        /// </summary>
        /// <value>
        /// The access control service.
        /// </value>
        public virtual IAccessControlService AccessControlService { get; set; }

        /// <summary>
        /// Checks for read only mode.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="rules">The rules.</param>
        protected void SetIsReadOnly(IAccessSecuredViewModel model, IList<IAccessRule> rules)
        {
            var principal = SecurityService.GetCurrentPrincipal();
            var accessLevel = AccessControlService.GetAccessLevel(rules, principal);

            model.IsReadOnly = accessLevel != AccessLevel.ReadWrite;

            if (model.IsReadOnly)
            {
                Context.Messages.AddInfo(RootGlobalization.Message_ReadOnlyMode);
            }
        }        
    }
}