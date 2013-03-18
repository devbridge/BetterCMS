using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

using BetterCms.Api;

namespace BetterCms.Module.Root.Mvc.Adapters
{
    public static class AttributeAdapterHelper
    {
        /// <summary>
        /// Fires when the CMS host start.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public static void RegisterValidationAdapters(SingleItemEventArgs<Core.Environment.Host.ICmsHost> args)
        {
            DataAnnotationsModelValidatorProvider.RegisterAdapter(
                typeof(RequiredAttribute),
                typeof(DefaultRequiredAttributeAdapter));

            DataAnnotationsModelValidatorProvider.RegisterAdapter(
                typeof(RegularExpressionAttribute),
                typeof(DefaultRegularExpressionAttributeAdapter));
            
            DataAnnotationsModelValidatorProvider.RegisterAdapter(
                typeof(RangeAttribute),
                typeof(DefaultRangeAttributeAdapter));
            
            DataAnnotationsModelValidatorProvider.RegisterAdapter(
                typeof(StringLengthAttribute),
                typeof(DefaultStringLengthAttributeAdapter));
        }

        /// <summary>
        /// Sets default validation attribute error message.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="metadata">The metadata.</param>
        /// <param name="attribute">The attribute.</param>
        /// <param name="resourceType">Type of the resource.</param>
        /// <param name="resourceName">Name of the resource.</param>
        public static void SetValidationAttributeErrorMessage<T>(ModelMetadata metadata, T attribute, Type resourceType, string resourceName)
            where T : ValidationAttribute
        {
            var useDefault = true;
            var attributes = metadata.ContainerType.GetProperty(metadata.PropertyName).GetCustomAttributes(typeof(T), true);
            if (attributes.Length > 0)
            {
                var customAttribute = (T)attributes[0];
                if (customAttribute.ErrorMessageResourceType != null && !string.IsNullOrWhiteSpace(attribute.ErrorMessageResourceName))
                {
                    attribute.ErrorMessageResourceType = customAttribute.ErrorMessageResourceType;
                    attribute.ErrorMessageResourceName = customAttribute.ErrorMessageResourceName;

                    useDefault = false;
                }
            }

            if (useDefault)
            {
                attribute.ErrorMessageResourceType = resourceType;
                attribute.ErrorMessageResourceName = resourceName;
            }
        }
    }
}