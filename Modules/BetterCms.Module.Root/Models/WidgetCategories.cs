// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WidgetCategories.cs" company="Devbridge Group LLC">
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

using BetterCms.Core.DataContracts;

using BetterModules.Core.DataContracts;
using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class WidgetCategory : EquatableEntity<WidgetCategory>, IEntityCategory
    {
        public virtual Category Category { get; set; }


        public virtual void SetEntity(IEntity entity)
        {
            Widget = entity as Widget;
        }

        public virtual Widget Widget { get; set; }

        ICategory IEntityCategory.Category
        {
            get
            {
                return Category;
            }
            set
            {
                Category = value as Category;
            }
        }

        IEntity IEntityCategory.Entity
        {
            get
            {
                return Widget;
            }
            set
            {
                Widget = value as Widget;
            }
        }

        public virtual WidgetCategory Clone()
        {
            return CopyDataTo(new WidgetCategory());
        }

        public virtual WidgetCategory CopyDataTo(WidgetCategory widget)
        {
            widget.Widget = Widget;
            widget.Category = Category;

            return widget;
        }
    }
}