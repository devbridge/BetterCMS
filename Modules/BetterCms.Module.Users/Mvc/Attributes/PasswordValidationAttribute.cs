// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PasswordValidationAttribute.cs" company="Devbridge Group LLC">
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

using BetterCms.Module.Root.Mvc;

using BetterCms.Module.Users.Content.Resources;
using BetterCms.Module.Users.ViewModels.User;

namespace BetterCms.Module.Users.Mvc.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class PasswordValidationAttribute : ValidationAttribute//, IClientValidatable
    {
        private readonly string RequiredMessage = UsersGlobalization.CreateUser_Password_IsRequired;
        private readonly string RegexMessage = UsersGlobalization.User_Password_LengthMessage;
        private const string RegExp = UsersModuleConstants.PasswordRegularExpression;
        private const string ClientValidationRule = "passwordvalidation";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var userViewModel = validationContext.ObjectInstance as EditUserViewModel;
            if (userViewModel != null && userViewModel.Id.HasDefaultValue() && value == null)
            {
                return new ValidationResult(RequiredMessage);
            }
            if (value != null)
            {
                var regEx = new Regex(RegExp);
                if (!regEx.IsMatch(value.ToString()))
                {
                    return new ValidationResult(RegexMessage);
                }
            }

            return ValidationResult.Success;
        }
// TODO:
//        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
//        {
//            var rule = new ModelClientValidationRule
//            {
//                ErrorMessage = RequiredMessage,
//                ValidationType = ClientValidationRule,
//            };
//            rule.ValidationParameters.Add("pattern", RegExp);
//            rule.ValidationParameters.Add("patternmessage", RegexMessage);
//
//            yield return rule;
//        }
    }
}