// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BlogChangingEventArgs.cs" company="Devbridge Group LLC">
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
using System.ComponentModel;

using BetterCms.Module.Blog.Models.Events;

// ReSharper disable CheckNamespace
namespace BetterCms.Events
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Blog post changing event arguments.
    /// </summary>
    public class BlogChangingEventArgs : CancelEventArgs
    {
        /// <summary>
        /// Gets the page model before update.
        /// </summary>
        /// <value>
        /// The page model before update.
        /// </value>
        public UpdatingBlogModel BeforeUpdate { get; private set; }

        /// <summary>
        /// Gets the page model after update.
        /// </summary>
        /// <value>
        /// The page model after update.
        /// </value>
        public UpdatingBlogModel AfterUpdate { get; private set; }

        /// <summary>
        /// Gets the cancellation messages.
        /// </summary>
        /// <value>
        /// The cancellation messages.
        /// </value>
        public List<string> CancellationErrorMessages { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogChangingEventArgs" /> class.
        /// </summary>
        /// <param name="beforeUpdate">The old page.</param>
        /// <param name="afterUpdate">The new page.</param>
        public BlogChangingEventArgs(UpdatingBlogModel beforeUpdate, UpdatingBlogModel afterUpdate)
        {
            BeforeUpdate = beforeUpdate;
            AfterUpdate = afterUpdate;
            CancellationErrorMessages = new List<string>();
        }

        /// <summary>
        /// Cancels the event with error message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="clearOtherMessages">if set to <c>true</c> clear other messages.</param>
        public void CancelWithErrorMessage(string message, bool clearOtherMessages = false)
        {
            if (clearOtherMessages)
            {
                CancellationErrorMessages.Clear();
            }

            Cancel = true;
            CancellationErrorMessages.Add(message);
        }
    }
}