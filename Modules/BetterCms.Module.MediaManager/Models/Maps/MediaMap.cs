// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MediaMap.cs" company="Devbridge Group LLC">
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

using BetterModules.Core.Models;

namespace BetterCms.Module.MediaManager.Models.Maps
{
    [Serializable]
    public class MediaMap : EntityMapBase<Media>
    {
        public MediaMap()
            : base(MediaManagerModuleDescriptor.ModuleName)
        {
            Table("Medias");

            Map(x => x.Title).Length(MaxLength.Name).Not.Nullable();
            Map(x => x.Description).Length(MaxLength.Text).Nullable();
            Map(x => x.IsArchived).Not.Nullable();
            Map(x => x.Type).Not.Nullable();
            Map(x => x.ContentType).Not.Nullable();
            Map(x => x.PublishedOn).Nullable();

            References(f => f.Folder).Cascade.SaveUpdate().LazyLoad().Nullable();
            References(f => f.Original).Cascade.SaveUpdate().LazyLoad().Nullable();

            References(f => f.Image).Cascade.SaveUpdate().LazyLoad();
            HasMany(x => x.MediaTags).KeyColumn("MediaId").Cascade.SaveUpdate().Inverse().LazyLoad().Where("IsDeleted = 0");
            HasMany(x => x.History).KeyColumn("OriginalId").Cascade.None().LazyLoad().Where("IsDeleted = 0");
            HasMany(x => x.Categories).KeyColumn("MediaId").Cascade.SaveUpdate().Inverse().LazyLoad().Where("IsDeleted = 0");
        }
    }
}