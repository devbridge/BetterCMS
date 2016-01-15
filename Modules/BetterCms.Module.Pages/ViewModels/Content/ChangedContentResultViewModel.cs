// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChangedContentResultViewModel.cs" company="Devbridge Group LLC">
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

using BetterCms.Core.DataContracts.Enums;

namespace BetterCms.Module.Pages.ViewModels.Content
{
    public class ChangedContentResultViewModel
    {
        public string Title { get; set; }
        
        public Guid PageId { get; set; }

        public Guid ContentId { get; set; }

        public Guid PageContentId { get; set; }

        public Guid RegionId { get; set; }

        public ContentStatus DesirableStatus { get; set; }

        public string ContentType { get; set; }
        
        public int ContentVersion { get; set; }
        
        public int PageContentVersion { get; set; }

        public List<PageContentChildRegionViewModel> Regions { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, Title: {1}, PageId: {2}, RegionId: {3}, ContentId: {4}, PageContentId: {5}", 
                base.ToString(), Title, PageId, RegionId, ContentId, PageContentId);
        }
    }
}