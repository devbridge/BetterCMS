using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

using BetterCms.Module.Root.Content.Resources;

namespace BetterCms.Module.Root.Mvc.Adapters
{
    public class DefaultStringLengthAttributeAdapter : StringLengthAttributeAdapter
    {
        public DefaultStringLengthAttributeAdapter(ModelMetadata metadata, ControllerContext context, StringLengthAttribute attribute)
            : base(metadata, context, attribute)
        {
            AttributeAdapterHelper.SetValidationAttributeErrorMessage(metadata, attribute, typeof(RootGlobalization), "Validation_StringLengthAttribute_Message");
        }
    }
}