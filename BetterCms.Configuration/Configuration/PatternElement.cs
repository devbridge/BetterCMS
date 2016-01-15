// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PatternElement.cs" company="Devbridge Group LLC">
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
using System.Configuration;

namespace BetterCms.Configuration
{
    public class PatternElement : ConfigurationElement
    {
        private const string ExpressionAttribute = "expression";
        private const string NegateAttribute = "negate";
        private const string DescriptionAttribute = "description";
        private const string IgnoreCaseAttribute = "ignoreCase";

        /// <summary>
        /// Gets or sets the expression.
        /// </summary>
        /// <value>
        /// The expression.
        /// </value>
        [ConfigurationProperty(ExpressionAttribute, DefaultValue = "", IsRequired = true)]
        public string Expression
        {
            get { return Convert.ToString(this[ExpressionAttribute]); }
            set { this[ExpressionAttribute] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="PatternElement"/> should be negated.
        /// </summary>
        /// <value>
        ///   <c>true</c> if negate; otherwise, <c>false</c>.
        /// </value>
        [ConfigurationProperty(NegateAttribute, DefaultValue = false, IsRequired = false)]
        public bool Negate
        {
            get { return Convert.ToBoolean(this[NegateAttribute]); }
            set { this[NegateAttribute] = value; }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [ConfigurationProperty(DescriptionAttribute, DefaultValue = "", IsRequired = false)]
        public string Description
        {
            get { return Convert.ToString(this[DescriptionAttribute]); }
            set { this[DescriptionAttribute] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="PatternElement"/> should be checked by ignoring case.
        /// </summary>
        /// <value>
        ///   <c>true</c> if ignore case; otherwise, <c>false</c>.
        /// </value>
        [ConfigurationProperty(IgnoreCaseAttribute, DefaultValue = false, IsRequired = false)]
        public bool IgnoreCase
        {
            get { return Convert.ToBoolean(this[IgnoreCaseAttribute]); }
            set { this[IgnoreCaseAttribute] = value; }
        }
    }
}