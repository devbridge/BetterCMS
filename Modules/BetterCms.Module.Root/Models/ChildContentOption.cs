// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChildContentOption.cs" company="Devbridge Group LLC">
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

using BetterCms.Core;
using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;

using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class ChildContentOption : EquatableEntity<ChildContentOption>, IOptionValueEntity, IMultilingualOption
    {
        public ChildContentOption()
        {
            Translations = new List<ChildContentOptionTranslation>();
        }

        public virtual ChildContent ChildContent { get; set; }

        public virtual string Value { get; set; }

        public virtual string Key { get; set; }

        public virtual bool UseDefaultValue { get; set; }

        public virtual OptionType Type { get; set; }

        public virtual CustomOption CustomOption { get; set; }

        public virtual IList<ChildContentOptionTranslation> Translations { get; set; }

        IList<IOptionTranslation> IMultilingualOption.Translations
        {
            get
            {
                return Translations.Cast<IOptionTranslation>().ToList();
            }
            set
            {
                Translations = value.Cast<ChildContentOptionTranslation>().ToList();
            }
        }

        ICustomOption IOption.CustomOption
        {
            get
            {
                return CustomOption;
            }
            set
            {
                CustomOption = (CustomOption)value;
            }
        }

        public virtual ChildContentOption Clone()
        {
            return CopyDataTo(new ChildContentOption());
        }

        public virtual ChildContentOption CopyDataTo(ChildContentOption contentOption, bool copyCollections = true)
        {
            contentOption.Key = Key;
            contentOption.Type = Type;
            contentOption.Value = Value;
            contentOption.CustomOption = CustomOption;
            contentOption.UseDefaultValue = UseDefaultValue;

            if (copyCollections && Translations != null)
            {
                foreach (var childContentOptionTranslation in Translations)
                {
                    var clonedTranslation = childContentOptionTranslation.Clone();
                    clonedTranslation.ChildContentOption = contentOption;
                    contentOption.Translations.Add(clonedTranslation);
                }
            }

            return contentOption;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}, Key: {1}, Value: {2}, Type: {3}", base.ToString(), Key, Value, Type);
        }
    }
}