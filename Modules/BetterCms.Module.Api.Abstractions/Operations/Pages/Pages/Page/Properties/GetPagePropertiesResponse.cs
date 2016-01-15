// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetPagePropertiesResponse.cs" company="Devbridge Group LLC">
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
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Root;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties
{
    [DataContract]
    [System.Serializable]
    public class GetPagePropertiesResponse : ResponseBase<PagePropertiesModel>
    {
        /// <summary>
        /// Gets or sets the layout.
        /// </summary>
        /// <value>
        /// The layout.
        /// </value>
        [DataMember]
        public LayoutModel Layout { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        [DataMember]
        public IList<CategoryModel> Categories { get; set; }

        /// <summary>
        /// Gets or sets the list of tags.
        /// </summary>
        /// <value>
        /// The list of tags.
        /// </value>
        [DataMember]
        public System.Collections.Generic.List<TagModel> Tags { get; set; }

        /// <summary>
        /// Gets or sets the main image.
        /// </summary>
        /// <value>
        /// The main image.
        /// </value>
        [DataMember]
        public ImageModel MainImage { get; set; }

        /// <summary>
        /// Gets or sets the featured image.
        /// </summary>
        /// <value>
        /// The featured image.
        /// </value>
        [DataMember]
        public ImageModel FeaturedImage { get; set; }

        /// <summary>
        /// Gets or sets the secondary image.
        /// </summary>
        /// <value>
        /// The secondary image.
        /// </value>
        [DataMember]
        public ImageModel SecondaryImage { get; set; }

        /// <summary>
        /// Gets or sets the meta data.
        /// </summary>
        /// <value>
        /// The meta data.
        /// </value>
        [DataMember]
        public MetadataModel MetaData { get; set; }
        
        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        [DataMember]
        public LanguageModel Language { get; set; }

        /// <summary>
        /// Gets or sets the list of page contents.
        /// </summary>
        /// <value>
        /// The list of page contents.
        /// </value>
        [DataMember]
        public System.Collections.Generic.IList<PageContentModel> PageContents { get; set; }
        
        /// <summary>
        /// Gets or sets the list of page options.
        /// </summary>
        /// <value>
        /// The list of page options.
        /// </value>
        [DataMember]
        public System.Collections.Generic.IList<OptionValueModel> PageOptions { get; set; }
        
        /// <summary>
        /// Gets or sets the list of page translations.
        /// </summary>
        /// <value>
        /// The list of page translations.
        /// </value>
        [DataMember]
        public System.Collections.Generic.IList<PageTranslationModel> PageTranslations { get; set; }

        /// <summary>
        /// Gets or sets the access rules.
        /// </summary>
        /// <value>
        /// The access rules.
        /// </value>
        [DataMember]
        public System.Collections.Generic.IList<AccessRuleModel> AccessRules { get; set; }   
    }
}