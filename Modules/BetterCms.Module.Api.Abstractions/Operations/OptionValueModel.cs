// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OptionValueModel.cs" company="Devbridge Group LLC">
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
using System.Runtime.Serialization;

using BetterCms.Module.Api.Operations.Root;

namespace BetterCms.Module.Api.Operations
{
    [DataContract]
    [Serializable]
    public class OptionValueModel
    {
        /// <summary>
        /// Gets or sets the option key.
        /// </summary>
        /// <value>
        /// The option key.
        /// </value>
        [DataMember]
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the option value.
        /// </summary>
        /// <value>
        /// The option value.
        /// </value>
        [DataMember]
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the option default value.
        /// </summary>
        /// <value>
        /// The option default value.
        /// </value>
        [DataMember]
        public string DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets the option type.
        /// </summary>
        /// <value>
        /// The option type.
        /// </value>
        [DataMember]
        public OptionType Type { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use default value.
        /// </summary>
        /// <value>
        ///   <c>true</c> if to use default value; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool UseDefaultValue { get; set; }

        /// <summary>
        /// Gets or sets the custom type identifier.
        /// </summary>
        /// <value>
        /// The custom type identifier.
        /// </value>
        [DataMember]
        public string CustomTypeIdentifier { get; set; }
    }
}
