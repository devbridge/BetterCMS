// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DisallowNonAlphanumericAttribute.cs" company="Devbridge Group LLC">
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
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace BetterCms.Module.Root.Mvc.Attributes
{
    /// <summary>
    /// Validates the field to only contain alphanumeric characters.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class DisallowNonAlphanumericAttribute : ValidationAttribute, IClientValidatable
    {
        /// <summary>
        /// The client validation rule.
        /// </summary>
        private const string clientValidationRule = "disallownonalphanumeric";

        /// <summary>
        /// Determines whether the specified value of the object is valid.
        /// </summary>
        /// <param name="value">The value of the object to validate.</param>
        /// <returns>
        /// true if the specified value is valid; otherwise, false.
        /// </returns>
        public override bool IsValid(object value)
        {
            try
            {
                var input = value as string;

                if (string.IsNullOrWhiteSpace(input))
                {
                    return true;
                }
                var regex = new Regex(RootModuleConstants.AlphanumericExpression);
                return regex.IsMatch(input);
            }
            catch (Exception exception)
            {
                throw new Exception("An error occured in DisallowNonAlphanumericAttribute", exception);
            }
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
            var rule = new ModelClientValidationRule { ErrorMessage = ErrorMessageString ?? "Field invalid. Field can only contain alphanumeric characters: 'a-z', 'A-Z', '0-9'.", ValidationType = clientValidationRule };
            rule.ValidationParameters.Add("pattern", RootModuleConstants.AlphanumericExpression);

            yield return rule;
        }
    }
}