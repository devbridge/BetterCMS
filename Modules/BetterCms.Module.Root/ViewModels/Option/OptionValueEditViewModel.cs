// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OptionValueEditViewModel.cs" company="Devbridge Group LLC">
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
using System.Web.Mvc;

namespace BetterCms.Module.Root.ViewModels.Option
{
    public class OptionValueEditViewModel : OptionViewModel
    {
        /// <summary>
        /// Gets or sets the option value.
        /// </summary>
        /// <value>
        /// The option value.
        /// </value>
        [AllowHtml]
        public string OptionValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether option is editable.
        /// </summary>
        /// <value>
        /// <c>true</c> if option is editable; otherwise, <c>false</c>.
        /// </value>
        public bool CanEditOption { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use default value.
        /// </summary>
        /// <value>
        ///   <c>true</c> if use default value; otherwise, <c>false</c>.
        /// </value>
        public bool UseDefaultValue { get; set; }

        /// <summary>
        /// Gets or sets the custom option value title.
        /// </summary>
        /// <value>
        /// The custom option value title.
        /// </value>
        public string CustomOptionValueTitle { get; set; }

        /// <summary>
        /// Gets or sets the value translations.
        /// </summary>
        /// <value>
        /// The value translations.
        /// </value>
        public IList<OptionTranslationViewModel> ValueTranslations { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, OptionValue: {1}", base.ToString(), OptionValue);
        }
    }
}