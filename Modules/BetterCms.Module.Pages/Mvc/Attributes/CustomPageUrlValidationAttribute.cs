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