// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomPageUrlValidationAttribute.cs" company="Devbridge Group LLC">
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
using System.Web.Mvc;

using BetterCms.Module.Pages.Content.Resources;

namespace BetterCms.Module.Pages.Mvc.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class CustomPageUrlValidationAttribute : ValidationAttribute, IClientValidatable
    {
        private readonly string errorMessage = PagesGlobalization.PageProperties_PageUrl_InvalidMessage;
        private const string clientValidationRule = "pageurlvalidation";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var pageUrl = value as string;
            if (!string.IsNullOrWhiteSpace(pageUrl))
            {
                var regExpAttribute = new RegularExpressionAttribute(PagesConstants.InternalUrlRegularExpression);
                if (!regExpAttribute.IsValid(pageUrl))
                {
                    return new ValidationResult(errorMessage);
                }
            }

            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = errorMessage,
                ValidationType = clientValidationRule,
            };
            rule.ValidationParameters.Add("pattern", PagesConstants.InternalUrlRegularExpression);

            yield return rule;
        }
    }
}