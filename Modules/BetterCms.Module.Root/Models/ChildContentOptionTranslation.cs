// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChildContentOptionTranslation.cs" company="Devbridge Group LLC">
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

using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class ChildContentOptionTranslation : EquatableEntity<ChildContentOptionTranslation>, IOptionTranslationEntity
    {
        public virtual ChildContentOption ChildContentOption { get; set; }

        public virtual Language Language { get; set; }

        public virtual string Value { get; set; }

        string IOptionTranslation.Value
        {
            get
            {
                return Value;
            }
            set
            {
                Value = value;
            }
        }

        string IOptionTranslation.LanguageId
        {
            get
            {
                return Language.Id.ToString();
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
            return string.Format("{0}, ChildContentOption: {1}, Language: {2}, Value: {3}", base.ToString(), ChildContentOption, Language, Value);
        }

        ILanguage IOptionTranslationEntity.Language
        {
            get
            {
                return Language;
            }
            set
            {
                Language = (Language)value;
            }
        }

        IOptionEntity IOptionTranslationEntity.Option
        {
            get
            {
                return ChildContentOption;
            }
            set
            {
                ChildContentOption = (ChildContentOption)value;
            }
        }

        public virtual ChildContentOptionTranslation Clone()
        {
            return CopyDataTo(new ChildContentOptionTranslation());
        }

        private ChildContentOptionTranslation CopyDataTo(ChildContentOptionTranslation childContentOptionTranslation)
        {
            childContentOptionTranslation.ChildContentOption = ChildContentOption;
            childContentOptionTranslation.Value = Value;
            childContentOptionTranslation.Language = Language;

            return childContentOptionTranslation;
        }
    }
}