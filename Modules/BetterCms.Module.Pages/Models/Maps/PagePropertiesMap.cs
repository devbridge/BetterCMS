// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PagePropertiesMap.cs" company="Devbridge Group LLC">
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
using BetterModules.Core.Models;

namespace BetterCms.Module.Pages.Models.Maps
{
    public class PagePropertiesMap : EntitySubClassMapBase<PageProperties>
    {
        public PagePropertiesMap() : base(PagesModuleDescriptor.ModuleName)
        {
            Table("Pages");
            
            Map(x => x.Description).Nullable();
            Map(x => x.CustomJS).Length(int.MaxValue).Nullable();
            Map(x => x.CustomCss).Length(int.MaxValue).Nullable();
            Map(x => x.UseCanonicalUrl).Not.Nullable();
            Map(x => x.UseNoFollow).Not.Nullable();
            Map(x => x.UseNoIndex).Not.Nullable();
            Map(x => x.IsArchived).Not.Nullable();

            
            References(x => x.Image).Cascade.SaveUpdate().LazyLoad();
            References(x => x.SecondaryImage).Cascade.SaveUpdate().LazyLoad();
            References(x => x.FeaturedImage).Cascade.SaveUpdate().LazyLoad();

            HasMany(x => x.PageTags).KeyColumn("PageId").Cascade.SaveUpdate().Inverse().LazyLoad().Where("IsDeleted = 0");
            HasMany(x => x.Categories).KeyColumn("PageId").Cascade.SaveUpdate().Inverse().LazyLoad().Where("IsDeleted = 0");            
        }
    }
}