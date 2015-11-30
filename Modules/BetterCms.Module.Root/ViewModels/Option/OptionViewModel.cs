// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OptionViewModel.cs" company="Devbridge Group LLC">
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
using System.Linq;
using System.Web.Mvc;

using BetterCms.Core;
using BetterCms.Core.DataContracts;

namespace BetterCms.Module.Root.ViewModels.Option
{
    /// <summary>
    /// Option view model
    /// </summary>
    public class OptionViewModel : OptionViewModelBase, IOption, IMultilingualOption
    {
        /// <summary>
        /// Gets or sets the option default value.
        /// </summary>
        /// <value>
        /// The option default value.
        /// </value>
        [AllowHtml]
        public string OptionDefaultValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether option is deletable.
        /// </summary>
        /// <value>
        /// <c>true</c> if option is deletable; otherwise, <c>false</c>.
        /// </value>
        public bool CanDeleteOption { get; set; }

        /// <summary>
        /// Gets or sets the custom option default value title.
        /// </summary>
        /// <value>
        /// The custom option default value title.
        /// </value>
        public string CustomOptionDefaultValueTitle { get; set; }

        /// <summary>
        /// Gets or sets the type of the custom.
        /// </summary>
        /// <value>
        /// The type of the custom.
        /// </value>
        // ReSharper disable UnusedMember.Global
        public string CustomType
        // ReSharper restore UnusedMember.Global
        {
            get
            {
                return CustomOption != null ? CustomOption.Identifier : null;
            }
            set
            {
                if (CustomOption == null)
                {
                    CustomOption = new CustomOptionViewModel();
                }
                CustomOption.Identifier = value;
            }
        }

        /// <summary>
        /// Gets or sets the translations.
        /// </summary>
        /// <value>
        /// The translations.
        /// </value>
        public IList<OptionTranslationViewModel> Translations { get; set; }

        IList<IOptionTranslation> IMultilingualOption.Translations
        {
            get
            {
                return Translations.Cast<IOptionTranslation>().ToList();
            }
            set
            {
                Translations = value.Cast<OptionTranslationViewModel>().ToList();
            }
        }

        /// <summary>
        /// Gets or sets the custom option.
        /// </summary>
        /// <value>
        /// The custom option.
        /// </value>
        /// <exception cref="System.NotSupportedException"></exception>
        ICustomOption IOption.CustomOption
        {
            get
            {
                return CustomOption;
            }
            set
            {
                throw new NotSupportedException("IOption.CustomOption has no setter. Use view model");
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("OptionKey: {0}, OptionDefaultValue: {1}, Type: {2}", OptionKey, OptionDefaultValue, Type);
        }

        #region IOption Members
        string IOption.Key
        {
            get
            {
                return OptionKey;
            }
            set
            {
                OptionKey = value;
            }
        }

        Core.DataContracts.Enums.OptionType IOption.Type
        {
            get
            {
                return Type;
            }
            set
            {
                Type = value;
            }
        }

        string IOption.Value
        {
            get
            {
                return OptionDefaultValue;
            }
            set
            {
                OptionDefaultValue = value;
            }
        }

        #endregion
    }
}