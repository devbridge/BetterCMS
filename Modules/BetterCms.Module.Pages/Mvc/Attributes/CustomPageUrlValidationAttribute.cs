using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

using BetterCms.Module.Pages.Content.Resources;

namespace BetterCms.Module.Pages.Mvc.Attributes
{
    public class CustomPageUrlValidationAttribute : ValidationAttribute, IClientValidatable
    {
        private readonly string errorMessage = PagesGlobalization.AddNewPageProperties_PagePermalink_InvalidMessage;
        private const string clientValidationRule = "pageurlvalidation";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var pageUrl = value as string;
            if (!string.IsNullOrWhiteSpace(pageUrl))
            {
                var regExpAttribute = new RegularExpressionAttribute(PagesConstants.PageUrlRegularExpression);
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
            rule.ValidationParameters.Add("pattern", PagesConstants.PageUrlRegularExpression);

            yield return rule;
        }
    }
}