// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Widget.cs" company="Devbridge Group LLC">
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

using BetterCms.Core.DataContracts;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class Widget : Content, IWidget, ICategorized
    {
        public virtual IList<WidgetCategory> Categories { get; set; }
        public const string CategorizableItemKeyForWidgets = "Widgets";


        IEnumerable<IEntityCategory> ICategorized.Categories
        {
            get
            {
                return Categories;
            }
        }

        public virtual string GetCategorizableItemKey()
        {
            return CategorizableItemKeyForWidgets;
        }

        public override Content CopyDataTo(Content content, bool copyCollections = true)
        {
            var copy = (Widget)base.CopyDataTo(content, copyCollections);

            if (copyCollections && Categories != null)
            {
                if (copy.Categories == null)
                {
                    copy.Categories = new List<WidgetCategory>();
                }

                foreach (var category in Categories)
                {
                    var clonedWidget = category.Clone();
                    clonedWidget.Widget = copy;

                    copy.Categories.Add(clonedWidget);
                }
            }

            return copy;
        }

        public override Content Clone(bool copyCollections = true)
        {
            return CopyDataTo(new Widget(), copyCollections);
        }


        public virtual void AddCategory(IEntityCategory category)
        {
            if (Categories == null)
            {
                Categories = new List<WidgetCategory>();
            }

            Categories.Add(category as WidgetCategory);
        }

        public virtual void RemoveCategory(IEntityCategory category)
        {
            if (Categories != null)
            {
                Categories.Remove(category as WidgetCategory);
            }
        }
    }
}