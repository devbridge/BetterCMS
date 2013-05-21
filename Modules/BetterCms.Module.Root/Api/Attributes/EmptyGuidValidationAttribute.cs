using System;
using System.ComponentModel.DataAnnotations;

using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Root.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class EmptyGuidValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is Guid && ((Guid)value).HasDefaultValue())
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}