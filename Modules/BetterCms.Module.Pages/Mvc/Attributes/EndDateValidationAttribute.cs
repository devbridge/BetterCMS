using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace BetterCms.Module.Pages.Mvc.Attributes
{
    /// <summary>
    /// Date validation attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public sealed class EndDateValidationAttribute : ValidationAttribute, IClientValidatable
    {
        /// <summary>
        /// The client validation rule.
        /// </summary>
        private const string clientValidationRule = "enddatevalidation";

        /// <summary>
        /// Gets or sets the date from property.
        /// </summary>
        /// <value>
        /// The date from property.
        /// </value>
        public string StartDateProperty { get; set; }

        /// <summary>
        /// Determines whether the specified value of the object is valid.
        /// </summary>
        /// <param name="value">The value of the object to validate.</param>
        /// <returns>
        /// true if the specified value is valid; otherwise, false.
        /// </returns>
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                var dateFromString = HttpContext.Current.Request[StartDateProperty];
                var dateTo = value as DateTime?;
                if (dateTo.HasValue)
                {
                    DateTime dateFrom;
                    if (DateTime.TryParse(dateFromString, out dateFrom))
                    {
                        return dateFrom <= dateTo;
                    }
                }
            }

            return true;
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
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ErrorMessageString,
                ValidationType = clientValidationRule,
            };
            rule.ValidationParameters.Add("startdateproperty", StartDateProperty);

            yield return rule;
        }
    }
}