using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

using BetterCms.Module.Root.Mvc;

using BetterCms.Module.Users.Content.Resources;
using BetterCms.Module.Users.ViewModels;

namespace BetterCms.Module.Users.Mvc.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class PasswordValidationAttribute : ValidationAttribute, IClientValidatable
    {
        private readonly string Message = UsersGlobalization.CreateUser_Password_IsRequired;
        private const string ClientValidationRule = "passwordrequired";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var userViewModel = validationContext.ObjectInstance as EditUserViewModel;
            if (userViewModel != null && userViewModel.Id.HasDefaultValue() && value == null)
            {
                return new ValidationResult(Message);
            }

            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = Message,
                ValidationType = ClientValidationRule,
            };

            yield return rule;
        }
    }
}