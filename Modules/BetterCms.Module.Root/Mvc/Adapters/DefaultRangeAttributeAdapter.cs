using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

using BetterCms.Module.Root.Content.Resources;

namespace BetterCms.Module.Root.Mvc.Adapters
{
    public class DefaultRangeAttributeAdapter : RangeAttributeAdapter
    {
        public DefaultRangeAttributeAdapter(ModelMetadata metadata, ControllerContext context, RangeAttribute attribute)
            : base(metadata, context, attribute)
        {
            AttributeAdapterHelper.SetValidationAttributeErrorMessage(metadata, attribute, typeof(RootGlobalization), "Validation_RangeAttribute_Message");
        }
    }
}