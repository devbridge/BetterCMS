// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MediaImageMap.cs" company="Devbridge Group LLC">
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
    public class MediaImageMap : EntitySubClassMapBase<MediaImage>
    {
        public MediaImageMap()
            : base(MediaManagerModuleDescriptor.ModuleName)
        {
            Table("MediaImages");

            Map(f => f.Caption).Nullable().Length(MaxLength.Text);
            Map(f => f.ImageAlign).Nullable();
            Map(f => f.Width).Not.Nullable();
            Map(f => f.Height).Not.Nullable();
            Map(f => f.CropCoordX1).Nullable();
            Map(f => f.CropCoordY1).Nullable();
            Map(f => f.CropCoordX2).Nullable();
            Map(f => f.CropCoordY2).Nullable();
            Map(f => f.OriginalWidth).Not.Nullable();
            Map(f => f.OriginalHeight).Not.Nullable();
            Map(f => f.OriginalSize).Not.Nullable();
            Map(f => f.OriginalUri).Not.Nullable();
            Map(f => f.IsOriginalUploaded).Nullable();
            Map(f => f.PublicOriginallUrl).Not.Nullable();
            Map(f => f.ThumbnailWidth).Not.Nullable();
            Map(f => f.ThumbnailHeight).Not.Nullable();
            Map(f => f.ThumbnailSize).Not.Nullable();
            Map(f => f.ThumbnailUri).Not.Nullable();
            Map(f => f.IsThumbnailUploaded).Nullable();
            Map(f => f.PublicThumbnailUrl);
        }
    }
}