// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultRootApiOperations.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.Api.Operations.Root.Categories;
using BetterCms.Module.Api.Operations.Root.Categories.Category;
using BetterCms.Module.Api.Operations.Root.Languages;
using BetterCms.Module.Api.Operations.Root.Languages.Language;
using BetterCms.Module.Api.Operations.Root.Layouts;
using BetterCms.Module.Api.Operations.Root.Layouts.Layout;
using BetterCms.Module.Api.Operations.Root.Tags;
using BetterCms.Module.Api.Operations.Root.Version;

using ITagService = BetterCms.Module.Api.Operations.Root.Tags.Tag.ITagService;

namespace BetterCms.Module.Api.Operations.Root
{
    public class DefaultRootApiOperations : IRootApiOperations
    {
        public DefaultRootApiOperations(ITagsService tags, ITagService tag, IVersionService version, ILayoutsService layouts, ILayoutService layout,
            ICategoryTreesService categories, ICategoryTreeService category, ILanguagesService languages, ILanguageService language)
        {
            Tags = tags;
            Tag = tag;
            Categories = categories;
            Category = category;
            Version = version;
            Layouts = layouts;
            Layout = layout;
            Languages = languages;
            Language = language;
        }

        public ITagsService Tags
        {
            get; 
            private set;
        }

        public ITagService Tag
        {
            get;
            private set;
        }
        
        public ICategoryTreesService Categories
        {
            get; 
            private set;
        }

        public ICategoryTreeService Category
        {
            get;
            private set;
        }

        public ILanguagesService Languages
        {
            get; 
            private set;
        }

        public ILanguageService Language
        {
            get;
            private set;
        }

        public IVersionService Version
        {
            get;
            private set;
        }

        public ILayoutsService Layouts
        {
            get;
            private set;
        }

        public ILayoutService Layout
        {
            get;
            private set;
        }
    }
}