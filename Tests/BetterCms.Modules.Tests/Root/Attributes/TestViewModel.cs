using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Mvc.Attributes;

namespace BetterCms.Test.Module.Root.Attributes
{
    public class TestViewModel
    {
        [DisallowHtml(ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_DisallowHtml_Field_Message")]
        public string DisallowHtmlProperty { get; set; }
    }
}