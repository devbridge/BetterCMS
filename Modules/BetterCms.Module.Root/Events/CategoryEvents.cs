// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryEvents.cs" company="Devbridge Group LLC">
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
using BetterCms.Core.DataContracts;

using BetterModules.Events;

// ReSharper disable CheckNamespace
namespace BetterCms.Events
// ReSharper restore CheckNamespace
{    
    /// <summary>
    /// Attachable page events container
    /// </summary>
    public partial class RootEvents
    {
        /// <summary>
        /// Occurs when a category tree is created.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<ICategoryTree>> CategoryTreeCreated;

        /// <summary>
        /// Occurs when a category tree is updated.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<ICategoryTree>> CategoryTreeUpdated;

        /// <summary>
        /// Occurs when a category tree is removed.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<ICategoryTree>> CategoryTreeDeleted;

        /// <summary>
        /// Occurs when a category node is created.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<ICategory>> CategoryCreated;

        /// <summary>
        /// Occurs when a category node is updated.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<ICategory>> CategoryUpdated;

        /// <summary>
        /// Occurs when a category node is removed.
        /// </summary>
        public event DefaultEventHandler<SingleItemEventArgs<ICategory>> CategoryDeleted;

        public void OnCategoryTreeCreated(ICategoryTree categoryTree)
        {
            if (CategoryTreeCreated != null)
            {
                CategoryTreeCreated(new SingleItemEventArgs<ICategoryTree>(categoryTree));
            }
        }

        public void OnCategoryTreeUpdated(ICategoryTree categoryTree)
        {
            if (CategoryTreeUpdated != null)
            {
                CategoryTreeUpdated(new SingleItemEventArgs<ICategoryTree>(categoryTree));
            }
        }

        public void OnCategoryTreeDeleted(ICategoryTree categoryTree)
        {
            if (CategoryTreeDeleted != null)
            {
                CategoryTreeDeleted(new SingleItemEventArgs<ICategoryTree>(categoryTree));
            }        
        }
        public void OnCategoryCreated(ICategory category)
        {
            if (CategoryCreated != null)
            {
                CategoryCreated(new SingleItemEventArgs<ICategory>(category));
            }
        }

        public void OnCategoryUpdated(ICategory category)
        {
            if (CategoryUpdated != null)
            {
                CategoryUpdated(new SingleItemEventArgs<ICategory>(category));
            }
        }

        public void OnCategoryDeleted(ICategory category)
        {
            if (CategoryDeleted != null)
            {
                CategoryDeleted(new SingleItemEventArgs<ICategory>(category));
            }        
        }
    }
}
