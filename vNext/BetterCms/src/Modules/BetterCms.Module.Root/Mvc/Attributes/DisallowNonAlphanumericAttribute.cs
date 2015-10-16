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