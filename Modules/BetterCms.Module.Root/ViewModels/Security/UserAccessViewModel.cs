// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserAccessViewModel.cs" company="Devbridge Group LLC">
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
using System.Web.Mvc;

using BetterCms.Core.Security;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Mvc.Attributes;

namespace BetterCms.Module.Root.ViewModels.Security
{
    [Serializable]
    public class UserAccessViewModel : IAccessRule
    {
        public Guid Id { get; set; }        

        [AllowHtml]
        [DisallowNonActiveDirectoryNameCompliantAttribute(ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_ActiveDirectoryCompliant_Message")]
        public string Identity { get; set; }

        public AccessLevel AccessLevel { get; set; }

        public bool IsForRole { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAccessViewModel" /> class.
        /// </summary>
        public UserAccessViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAccessViewModel" /> class.
        /// </summary>
        /// <param name="accessRule">The access rule.</param>
        public UserAccessViewModel(IAccessRule accessRule)
        {
            Id = accessRule.Id;
            Identity = accessRule.Identity;
            AccessLevel = accessRule.AccessLevel;
            IsForRole = accessRule.IsForRole;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Id: {0}, Identity: {1}, AccessLevel: {2}", Id, Identity, AccessLevel);
        }
    }
}