using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BetterCms.Module.Pages.Mvc.Attributes
{
    public class DateValidationAttribute : ValidationAttribute, IClientValidatable
    {
        private readonly string errorMessage;

        public DateValidationAttribute(string errorMessage)
        {
            this.errorMessage = errorMessage;
        }

        public override bool IsValid(object value)
        {
            return true;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule { ErrorMessage = errorMessage, ValidationType = "datevalidation", };

            yield return rule;
        }
    }
}