using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BetterCms.Module.Root.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class AllowableValuesValidationAttribute : ValidationAttribute
    {
        private readonly object[] allowableObjects;
        
        private bool allow;

        public AllowableValuesValidationAttribute(object[] allowableObjects, bool allow = true)
        {
            this.allowableObjects = allowableObjects;
            this.allow = allow;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (allow && !allowableObjects.Contains(value))
            {
                return new ValidationResult(ErrorMessage);
            }
            if (!allow && allowableObjects.Contains(value))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}