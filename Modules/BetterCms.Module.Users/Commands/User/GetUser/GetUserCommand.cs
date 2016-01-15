// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetUserCommand.cs" company="Devbridge Group LLC">
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
using System.Linq;

using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.MediaManager.ViewModels;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Users.ViewModels.User;

using BetterModules.Core.DataAccess.DataContext;
using BetterModules.Core.Web.Mvc.Commands;

using NHibernate.Linq;

namespace BetterCms.Module.Users.Commands.User.GetUser
{
    /// <summary>
    /// Command for getting user view model
    /// </summary>
    public class GetUserCommand : CommandBase, ICommand<Guid, EditUserViewModel>
    {
        /// <summary>
        /// The file URL resolver
        /// </summary>
        private readonly IMediaFileUrlResolver fileUrlResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUserCommand"/> class.
        /// </summary>
        /// <param name="fileUrlResolver">The file URL resolver.</param>
        public GetUserCommand(IMediaFileUrlResolver fileUrlResolver)
        {
            this.fileUrlResolver = fileUrlResolver;
        }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public EditUserViewModel Execute(Guid userId)
        {
            EditUserViewModel model;
            
            if (!userId.HasDefaultValue())
            {
                var listFuture = Repository.AsQueryable<Models.User>()
                    .Where(bp => bp.Id == userId)
                    .Select(
                        user =>
                        new EditUserViewModel
                            {
                                Id = user.Id,
                                Version = user.Version,
                                FirstName = user.FirstName,
                                Email = user.Email,
                                LastName = user.LastName,
                                UserName = user.UserName,
                                Image = user.Image != null && !user.Image.IsDeleted ?
                                    new ImageSelectorViewModel
                                        {
                                            ImageId = user.Image.Id,
                                            ImageUrl = fileUrlResolver.EnsureFullPathUrl(user.Image.PublicUrl),
                                            ThumbnailUrl = fileUrlResolver.EnsureFullPathUrl(user.Image.PublicThumbnailUrl),
                                            ImageTooltip = user.Image.Caption,
                                            FolderId = user.Image.Folder != null ? user.Image.Folder.Id : (Guid?)null
                                        } : null
                            })
                    .ToFuture();

                var roles = Repository
                    .AsQueryable<Models.UserRole>()
                    .Where(ur => ur.User.Id == userId)
                    .Select(ur => ur.Role.Name)
                    .ToFuture();

                model = listFuture.FirstOne();
                model.Roles = roles.ToList();
            }
            else
            {
                model = new EditUserViewModel();
            }

            return model;
        }
    }
}