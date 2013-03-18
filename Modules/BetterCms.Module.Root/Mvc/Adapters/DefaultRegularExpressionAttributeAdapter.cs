using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

using BetterCms.Module.Root.Content.Resources;

namespace BetterCms.Module.Root.Mvc.Adapters
{
    public class DefaultRegularExpressionAttributeAdapter : RegularExpressionAttributeAdapter
    {
        public DefaultRegularExpressionAttributeAdapter(ModelMetadata metadata, ControllerContext context, RegularExpressionAttribute attribute)
            : base(metadata, context, attribute)
        {
            attribute.ErrorMessageResourceType = typeof(RootGlobalization);
            attribute.ErrorMessageResourceName = "Validation_RegularExpressionAttribute_Message";
        }
    }
}