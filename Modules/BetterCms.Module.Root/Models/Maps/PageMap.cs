// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageMap.cs" company="Devbridge Group LLC">
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

namespace BetterCms.Module.Root.Models.Maps
{
    public class PageMap : EntityMapBase<Page>
    {
        public PageMap()
            : base(RootModuleDescriptor.ModuleName)
        {
            Table("Pages");

            Map(x => x.PageUrl).Not.Nullable().Length(MaxLength.Url);
            Map(x => x.PageUrlHash).Not.Nullable().Length(MaxLength.UrlHash);
            Map(x => x.Title).Not.Nullable().Length(MaxLength.Name);
            Map(x => x.Status).Not.Nullable();
            Map(x => x.PublishedOn).Nullable();
            Map(x => x.MetaTitle).Length(MaxLength.Name);
            Map(x => x.MetaKeywords).Length(MaxLength.Max);
            Map(x => x.MetaDescription).Length(MaxLength.Max);
            Map(x => x.IsMasterPage).Not.Nullable();
            Map(x => x.LanguageGroupIdentifier).Nullable();
            Map(x => x.ForceAccessProtocol).Not.Nullable();

            References(x => x.Layout).Nullable().Cascade.SaveUpdate().LazyLoad();
            References(x => x.MasterPage).Nullable().Cascade.SaveUpdate().LazyLoad();
            References(x => x.Language).Nullable().Cascade.SaveUpdate().LazyLoad();

            References(x => x.PagesView).Column("Id").ReadOnly();
            
            HasMany(x => x.PageContents).Inverse().Cascade.SaveUpdate().LazyLoad().Where("IsDeleted = 0");
            HasMany(x => x.Options).KeyColumn("PageId").Cascade.SaveUpdate().Inverse().LazyLoad().Where("IsDeleted = 0");
            HasMany(x => x.MasterPages).KeyColumn("PageId").Cascade.SaveUpdate().Inverse().LazyLoad().Where("IsDeleted = 0");

            HasManyToMany(x => x.AccessRules).Table("PageAccessRules").Schema(SchemaName).Cascade.AllDeleteOrphan().LazyLoad();           
        }
    }
}
