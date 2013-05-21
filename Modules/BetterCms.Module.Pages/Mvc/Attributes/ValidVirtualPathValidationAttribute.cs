using System;
using System.ComponentModel.DataAnnotations;

using BetterCms.Module.Pages.Helpers;

namespace BetterCms.Module.Pages.Mvc.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class ValidVirtualPathValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var path = value as string;
            if (!string.IsNullOrWhiteSpace(path) && !HttpHelper.VirtualPathExists(path))
            {
                return new ValidationResult(string.Format(ErrorMessageString, path));
            }

            return ValidationResult.Success;
        }
    }
}