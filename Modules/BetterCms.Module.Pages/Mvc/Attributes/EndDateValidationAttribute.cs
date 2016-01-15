// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EndDateValidationAttribute.cs" company="Devbridge Group LLC">
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
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace BetterCms.Module.Pages.Mvc.Attributes
{
    /// <summary>
    /// Date validation attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class EndDateValidationAttribute : ValidationAttribute, IClientValidatable
    {
        /// <summary>
        /// The client validation rule.
        /// </summary>
        private const string clientValidationRule = "enddatevalidation";

        /// <summary>
        /// Gets or sets the date from property.
        /// </summary>
        /// <value>
        /// The date from property.
        /// </value>
        public string StartDateProperty { get; set; }

        /// <summary>
        /// Determines whether the specified value of the object is valid.
        /// </summary>
        /// <param name="value">The value of the object to validate.</param>
        /// <returns>
        /// true if the specified value is valid; otherwise, false.
        /// </returns>
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                var dateFromString = HttpContext.Current.Request[StartDateProperty];
                var dateTo = value as DateTime?;
                if (dateTo.HasValue)
                {
                    DateTime dateFrom;
                    if (DateTime.TryParse(dateFromString, out dateFrom))
                    {
                        return dateFrom <= dateTo;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// When implemented in a class, returns client validation rules for that class.
        /// </summary>
        /// <param name="metadata">The model metadata.</param>
        /// <param name="context">The controller context.</param>
        /// <returns>
        /// The client validation rules for this validator.
        /// </returns>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ErrorMessageString,
                ValidationType = clientValidationRule,
            };
            rule.ValidationParameters.Add("startdateproperty", StartDateProperty);

            yield return rule;
        }
    }
}