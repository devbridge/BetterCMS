// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlogEvents.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.Blog.Models.Events;
using BetterCms.Module.Pages.Models;

using BetterModules.Events;

// ReSharper disable CheckNamespace
namespace BetterCms.Events
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Attachable blog events container
    /// </summary>
    public partial class BlogEvents
    {
        /// <summary>
        /// Occurs when blog post is created.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<BlogPost>> BlogCreated;

        /// <summary>
        /// Occurs before blog post is updated.
        /// </summary>
        public event DefaultEventHandler<BlogChangingEventArgs> BlogChanging;

        /// <summary>
        /// Occurs when blog post is updated.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<BlogPost>> BlogUpdated;

        /// <summary>
        /// Occurs when blog post is deleted.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<BlogPost>> BlogDeleted;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogEvents" /> class.
        /// </summary>
        public BlogEvents()
        {
            PageEvents.Instance.PageDeleted += OnPageDeleted;
        }

        /// <summary>
        /// Called when a blog is created.
        /// </summary>
        public void OnBlogCreated(BlogPost blog)
        {
            if (BlogCreated != null)
            {
                BlogCreated(new SingleItemEventArgs<BlogPost>(blog));
            }
        }

        public BlogChangingEventArgs OnBlogChanging(UpdatingBlogModel beforeUpdate, UpdatingBlogModel afterUpdate)
        {
            var args = new BlogChangingEventArgs(beforeUpdate, afterUpdate);

            if (BlogChanging != null)
            {
                BlogChanging(args);
            }

            return args;
        }

        /// <summary>
        /// Called when a blog is created.
        /// </summary>
        public void OnBlogUpdated(BlogPost blog)
        {
            if (BlogUpdated != null)
            {
                BlogUpdated(new SingleItemEventArgs<BlogPost>(blog));
            }
        }

        /// <summary>
        /// Called when a blog is deleted.
        /// </summary>
        public void OnBlogDeleted(BlogPost blog)
        {
            if (BlogDeleted != null)
            {
                BlogDeleted(new SingleItemEventArgs<BlogPost>(blog));
            }
        }

        private void OnPageDeleted(SingleItemEventArgs<PageProperties> args)
        {
            if (args != null && args.Item is BlogPost)
            {
                OnBlogDeleted((BlogPost)args.Item);
            }
        }
    }
}
