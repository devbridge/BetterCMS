// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StubMappingResolver.cs" company="Devbridge Group LLC">
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
using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Module.Blog.Models.Maps;
using BetterCms.Module.MediaManager.Models.Maps;
using BetterCms.Module.Newsletter.Models.Maps;
using BetterCms.Module.Pages.Models.Maps;
using BetterCms.Module.Root.Models.Maps;
using BetterCms.Module.Users.Models.Maps;

using FluentNHibernate.Cfg;

namespace BetterCms.Test.Module.Helpers
{
    public class StubMappingResolver : IMappingResolver
    {
        public void AddAvailableMappings(FluentConfiguration fluentConfiguration)
        {
            fluentConfiguration.Mappings(mc => mc.FluentMappings.AddFromAssemblyOf<PagePropertiesMap>());
            fluentConfiguration.Mappings(mc => mc.FluentMappings.AddFromAssemblyOf<PageMap>());
            fluentConfiguration.Mappings(mc => mc.FluentMappings.AddFromAssemblyOf<MediaMap>());
            fluentConfiguration.Mappings(mc => mc.FluentMappings.AddFromAssemblyOf<BlogPostMap>());
            fluentConfiguration.Mappings(mc => mc.FluentMappings.AddFromAssemblyOf<RoleMap>());
            fluentConfiguration.Mappings(mc => mc.FluentMappings.AddFromAssemblyOf<SubscriberMap>());
        }
    }
}
