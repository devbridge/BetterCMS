// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MediaFileMap.cs" company="Devbridge Group LLC">
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

namespace BetterCms.Module.MediaManager.Models.Maps
{
    public class MediaFileMap : EntitySubClassMapBase<MediaFile>
    {
        public MediaFileMap()
            : base(MediaManagerModuleDescriptor.ModuleName)
        {
            Table("MediaFiles");            

            Map(f => f.OriginalFileName).Not.Nullable().Length(MaxLength.Name);
            Map(f => f.OriginalFileExtension).Nullable().Length(MaxLength.Name);
            Map(f => f.FileUri).Not.Nullable().Length(MaxLength.Uri);
            Map(f => f.PublicUrl).Not.Nullable().Length(MaxLength.Url);
            Map(f => f.Size).Not.Nullable();
            Map(f => f.IsTemporary).Not.Nullable().Default("1");
            Map(f => f.IsUploaded).Nullable();
            Map(f => f.IsCanceled).Not.Nullable().Default("0");
            Map(f => f.IsMovedToTrash).Not.Nullable().Default("0");
            Map(f => f.NextTryToMoveToTrash).Nullable();

            HasManyToMany(x => x.AccessRules).Table("MediaFileAccessRules").Schema(SchemaName).Cascade.AllDeleteOrphan().LazyLoad();
        }
    }
}