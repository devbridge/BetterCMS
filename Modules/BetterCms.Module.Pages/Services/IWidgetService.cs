// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWidgetService.cs" company="Devbridge Group LLC">
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

using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Content;
using BetterCms.Module.Pages.ViewModels.Filter;
using BetterCms.Module.Pages.ViewModels.SiteSettings;
using BetterCms.Module.Pages.ViewModels.Widgets;

using BetterCms.Module.Root.ViewModels.Option;

namespace BetterCms.Module.Pages.Services
{
    public interface IWidgetService
    {
        void SaveHtmlContentWidget(EditHtmlContentWidgetViewModel model, IList<ContentOptionValuesViewModel> childContentOptionValues, out HtmlContentWidget widget, out HtmlContentWidget originalWidget, bool treatNullsAsLists = true, bool createIfNotExists = false);

        ServerControlWidget SaveServerControlWidget(EditServerControlWidgetViewModel model, bool treatNullsAsLists = true, bool createIfNotExists = false);

        bool DeleteWidget(System.Guid widgetId, int widgetVersion);

        SiteSettingWidgetListViewModel GetFilteredWidgetsList(WidgetsFilter filter);

        List<PageContentChildRegionViewModel> GetWidgetChildRegionViewModels(Root.Models.Content content);
    }
}