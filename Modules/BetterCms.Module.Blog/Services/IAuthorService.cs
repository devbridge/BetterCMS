// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAuthorService.cs" company="Devbridge Group LLC">
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
using System.Collections.Generic;

using BetterCms.Module.Blog.Models;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Blog.Services
{
    public interface IAuthorService
    {
        /// <summary>
        /// Gets the list of author lookup values.
        /// </summary>
        /// <returns>List of author lookup values.</returns>
        IEnumerable<LookupKeyValue> GetAuthors();

        void DeleteAuthor(Guid authorId, int version);

        Author CreateAuthor(string name, Guid? imageId, string description);

        Author UpdateAuthor(Guid authorId, int version, string name, Guid? imageId, string description);
    }
}