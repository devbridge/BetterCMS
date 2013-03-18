using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

using BetterCms.Module.Root.Content.Resources;

namespace BetterCms.Module.Root.Mvc.Adapters
{
    public class DefaultRequiredAttributeAdapter : RequiredAttributeAdapter
    {
        public DefaultRequiredAttributeAdapter(ModelMetadata metadata, ControllerContext context, RequiredAttribute attribute)
            : base(metadata, context, attribute)
        {
            AttributeAdapterHelper.SetValidationAttributeErrorMessage(metadata, attribute, typeof(RootGlobalization), "Validation_RequiredAttribute_Message");
        }
    }
}