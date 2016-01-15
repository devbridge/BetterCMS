// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthorEvents.cs" company="Devbridge Group LLC">
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

using BetterModules.Events;

// ReSharper disable CheckNamespace
namespace BetterCms.Events
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Author events container.
    /// </summary>
    public partial class BlogEvents : EventsBase<BlogEvents>
    {
        /// <summary>
        /// Occurs when an author is created.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Author>> AuthorCreated;

        /// <summary>
        /// Occurs when an author is updated.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Author>> AuthorUpdated;

        /// <summary>
        /// Occurs when an author is deleted.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<Author>> AuthorDeleted;
        
        public void OnAuthorCreated(Author author)
        {
            if (AuthorCreated != null)
            {
                AuthorCreated(new SingleItemEventArgs<Author>(author));
            }
        }
        
        public void OnAuthorUpdated(Author author)
        {
            if (AuthorUpdated != null)
            {
                AuthorUpdated(new SingleItemEventArgs<Author>(author));
            }
        }

        public void OnAuthorDeleted(Author author)
        {
            if (AuthorDeleted != null)
            {
                AuthorDeleted(new SingleItemEventArgs<Author>(author));
            }
        }
    }
}
