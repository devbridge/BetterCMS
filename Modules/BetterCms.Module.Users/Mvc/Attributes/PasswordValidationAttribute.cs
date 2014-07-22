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