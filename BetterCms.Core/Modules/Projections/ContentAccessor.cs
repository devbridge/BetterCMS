// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContentAccessor.cs" company="Devbridge Group LLC">
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
using System.Web.Mvc;

using BetterCms.Core.DataContracts;

namespace BetterCms.Core.Modules.Projections
{
    public interface IJavaScriptAccessor
    {
        string[] GetCustomJavaScript(HtmlHelper html);

        string[] GetJavaScriptResources(HtmlHelper html);
    }

    public interface IStylesheetAccessor
    {
        string[] GetCustomStyles(HtmlHelper html);

        string[] GetStylesResources(HtmlHelper html);
    }

    public interface IHtmlAccessor
    {
        string GetContentWrapperType();

        string GetHtml(HtmlHelper html);

        string GetTitle();
    }

    public interface IContentAccessor : IHtmlAccessor, IStylesheetAccessor, IJavaScriptAccessor
    {
        PropertiesPreview GetHtmlPropertiesPreview();
    }

    [Serializable]
    public abstract class ContentAccessor<TContent> :  IContentAccessor 
        where TContent : IContent
    {
        protected TContent Content { get; private set; }

        protected IList<IOptionValue> Options { get; private set; }
        
        protected ContentAccessor(TContent content, IList<IOptionValue> options)
        {
            Content = content;
            Options = options;
        }

        public virtual string GetTitle()
        {
            return Content.Name;
        }

        public abstract string GetContentWrapperType();

        public abstract string GetHtml(HtmlHelper html);

        public abstract string[] GetCustomStyles(HtmlHelper html);

        public abstract string[] GetCustomJavaScript(HtmlHelper html);

        public abstract string[] GetStylesResources(HtmlHelper html);

        public abstract string[] GetJavaScriptResources(HtmlHelper html);

        public virtual PropertiesPreview GetHtmlPropertiesPreview()
        {
            return null;
        }
    }
}