using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Content.Resources;

namespace BetterCms.Module.Root.Registration
{
    public class HtmlEditorJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        public HtmlEditorJsModuleIncludeDescriptor(RootModuleDescriptor module)
            : base(module, "bcms.htmlEditor")
        {

            Links = new IActionProjection[]
                {                       
                };

            Globalization = new IActionProjection[]
                {
                    new JavaScriptModuleGlobalization(this, "smartTagPageTitle", () => RootGlobalization.SmartTag_PageTitle_Title),
                    new JavaScriptModuleGlobalization(this, "smartTagPageUrl", () => RootGlobalization.SmartTag_PageUrl_Title),
                    new JavaScriptModuleGlobalization(this, "smartTagPageId", () => RootGlobalization.SmartTag_PageId_Title),
                    new JavaScriptModuleGlobalization(this, "smartTagPageCreatedOn", () => RootGlobalization.SmartTag_PageCreatedOn_Title),
                    new JavaScriptModuleGlobalization(this, "smartTagPageModifiedOn", () => RootGlobalization.SmartTag_PageModifiedOn_Title),
                    new JavaScriptModuleGlobalization(this, "smartTagPageOption", () => RootGlobalization.SmartTag_PageOption_Title),
                    new JavaScriptModuleGlobalization(this, "smartTagWidgetOption", () => RootGlobalization.SmartTag_WidgetOption_Title),
                    new JavaScriptModuleGlobalization(this, "smartTagPageMetaTitle", () => RootGlobalization.SmartTag_PageMetaTitle_Title),
                    new JavaScriptModuleGlobalization(this, "smartTagPageMetaKeywords", () => RootGlobalization.SmartTag_PageMetaKeywords_Title),
                    new JavaScriptModuleGlobalization(this, "smartTagPageMetaDescription", () => RootGlobalization.SmartTag_PageMetaDescription_Title),
                    new JavaScriptModuleGlobalization(this, "smartTagPageMainImageUrl", () => RootGlobalization.SmartTag_PageMainImageUrl_Title),
                    new JavaScriptModuleGlobalization(this, "smartTagPageSecondaryImageUrl", () => RootGlobalization.SmartTag_PageSecondaryImageUrl_Title),
                    new JavaScriptModuleGlobalization(this, "smartTagPageFeaturedImageUrl", () => RootGlobalization.SmartTag_PageFeaturedImageUrl_Title),
                    new JavaScriptModuleGlobalization(this, "smartTagPageCategory", () => RootGlobalization.SmartTag_PageCategory_Title),
                    new JavaScriptModuleGlobalization(this, "smartTagBlogAuthor", () => RootGlobalization.SmartTag_BlogAuthor_Title),
                    new JavaScriptModuleGlobalization(this, "smartTagBlogActivationDate", () => RootGlobalization.SmartTag_BlogActivationDate_Title),
                    new JavaScriptModuleGlobalization(this, "smartTagBlogExpirationDate", () => RootGlobalization.SmartTag_BlogExpirationDate_Title)
                };
        }
    }
}