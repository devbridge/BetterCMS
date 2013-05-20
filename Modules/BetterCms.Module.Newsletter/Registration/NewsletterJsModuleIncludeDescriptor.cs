using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Newsletter.Content.Resources;
using BetterCms.Module.Newsletter.Controllers;

namespace BetterCms.Module.Newsletter.Registration
{
    /// <summary>
    /// bcms.pages.js module descriptor.
    /// </summary>
    public class NewsletterJsModuleIncludeDescriptor : JsIncludeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NewsletterJsModuleIncludeDescriptor" /> class.
        /// </summary>
        /// <param name="module">The container module.</param>
        public NewsletterJsModuleIncludeDescriptor(ModuleDescriptor module)
            : base(module, "bcms.newsletter")
        {
            Links = new IActionProjection[]
                {
                    new JavaScriptModuleLinkTo<SubscriberController>(this, "loadSiteSettingsSubscribersUrl", c => c.ListTemplate()),
                    new JavaScriptModuleLinkTo<SubscriberController>(this, "loadSubscribersUrl", c => c.SubscribersList(null)),
                    new JavaScriptModuleLinkTo<SubscriberController>(this, "saveSubscriberUrl", c => c.SaveSubscriber(null)),
                    new JavaScriptModuleLinkTo<SubscriberController>(this, "deleteSubscriberUrl", c => c.DeleteSubscriber(null, null)),
                };

            Globalization = new IActionProjection[]
                {
                    new JavaScriptModuleGlobalization(this, "deleteSubscriberDialogTitle", () => NewsletterGlobalization.DeleteSubscriber_Confirmation_Message), 
                };
        }
    }
}