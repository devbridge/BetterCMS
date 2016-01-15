// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SaveAuthorCommand.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.Services;
using BetterCms.Module.Blog.ViewModels.Author;

using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Blog.Commands.SaveAuthor
{
    public class SaveAuthorCommand : CommandBase, ICommand<AuthorViewModel, AuthorViewModel>
    {
        private IAuthorService authorService;

        public SaveAuthorCommand(IAuthorService authorService)
        {
            this.authorService = authorService;
        }

        public AuthorViewModel Execute(AuthorViewModel request)
        {
            Author author;

            if (request.Id.HasDefaultValue())
            {
                author = authorService.CreateAuthor(request.Name, request.Image != null ? request.Image.ImageId : null, request.Description);
            }
            else
            {
                author = authorService.UpdateAuthor(request.Id, request.Version, request.Name, request.Image != null ? request.Image.ImageId : null, request.Description);
            }

            return new AuthorViewModel {
                                           Id = author.Id,
                                           Version = author.Version,
                                           Name = author.Name,
                                           Description = author.Description
                                       };
        }
    }
}