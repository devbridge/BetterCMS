// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OptionValueViewModel.cs" company="Devbridge Group LLC">
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

namespace BetterCms.Module.Root.ViewModels.Option
{
    [Serializable]
    public class OptionValueViewModel : OptionViewModelBase, IOptionValue
    {
        /// <summary>
        /// Gets or sets the option value.
        /// </summary>
        /// <value>
        /// The option value.
        /// </value>
        public object OptionValue { get; set; }

        public bool UseDefaultValue { get; set; }
        /// <summary>
        /// Gets the option key.
        /// </summary>
        /// <value>
        /// The option key.
        /// </value>
        string IOption.Key
        {
            get
            {
                return OptionKey;
            }
            set
            {
                throw new NotImplementedException("Operation not supported"); 
            }
        }

        /// <summary>
        /// Gets the option value.
        /// </summary>
        /// <value>
        /// The option value.
        /// </value>
        string IOption.Value
        {
            get
            {
                return OptionValue != null ? OptionValue.ToString() : null;
            }
            set
            {
                throw new NotImplementedException("Operation not supported"); 
            }
        }

        /// <summary>
        /// Gets or sets the custom option.
        /// </summary>
        /// <value>
        /// The custom option.
        /// </value>
        ICustomOption IOption.CustomOption
        {
            get
            {
                return CustomOption;
            }
            set
            {
                throw new NotSupportedException("IOptionValue.CustomOption has no setter. Use view model");
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
            return string.Format("OptionKey: {0}, OptionValue: {1}, Type: {2}", OptionKey, OptionValue, Type);
        }
    }
}