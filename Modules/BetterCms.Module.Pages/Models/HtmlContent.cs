// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HtmlContent.cs" company="Devbridge Group LLC">
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

using BetterCms.Core.DataContracts;
using BetterCms.Module.Pages.Models.Enums;

namespace BetterCms.Module.Pages.Models
{
    [Serializable]
    public class HtmlContent : Root.Models.Content, IHtmlContent, IDynamicContentContainer
    {
        public HtmlContent()
        {
            ContentTextMode = ContentTextMode.Html;
        }

        public virtual DateTime ActivationDate { get; set; }

        public virtual DateTime? ExpirationDate { get; set; }

        public virtual string CustomCss { get; set; }

        public virtual bool UseCustomCss { get; set; }

        public virtual string Html { get; set; }

        public virtual string OriginalText { get; set; }

        public virtual string CustomJs { get; set; }

        public virtual bool UseCustomJs { get; set; }
        
        public virtual bool EditInSourceMode { get; set; }

        public virtual ContentTextMode ContentTextMode { get; set; }

        public override Root.Models.Content CopyDataTo(Root.Models.Content content, bool copyCollections = true)
        {
            var copy = (HtmlContent)base.CopyDataTo(content, copyCollections);
            copy.ActivationDate = ActivationDate;
            copy.ExpirationDate = ExpirationDate;
            copy.CustomCss = CustomCss;
            copy.UseCustomCss = UseCustomCss;
            copy.Html = Html;            
            copy.CustomJs = CustomJs;
            copy.UseCustomJs = UseCustomJs;
            copy.EditInSourceMode = EditInSourceMode;
            copy.ContentTextMode = ContentTextMode;
            copy.OriginalText = OriginalText;

            return copy;
        }

        public override Root.Models.Content Clone(bool copyCollections = true)
        {
            return CopyDataTo(new HtmlContent(), copyCollections);
        }
    }
}