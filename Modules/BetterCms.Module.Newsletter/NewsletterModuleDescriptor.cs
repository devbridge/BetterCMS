using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Transactions;

using Autofac;

using BetterCms.Api;
using BetterCms.Core;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Modules;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Newsletter.Content.Resources;
using BetterCms.Module.Newsletter.Registration;
using BetterCms.Module.Newsletter.Services;
using BetterCms.Module.Pages.Api.Dto;
using BetterCms.Module.Root;

namespace BetterCms.Module.Newsletter
{
    /// <summary>
    /// Pages module descriptor.
    /// </summary>
    public class NewsletterModuleDescriptor : ModuleDescriptor
    {
        /// <summary>
        /// The module name.
        /// </summary>
        internal const string ModuleName = "newsletter";

        /// <summary>
        /// The newsletter java script module descriptor.
        /// </summary>
        private readonly NewsletterJsModuleIncludeDescriptor newsletterJsModuleIncludeDescriptor;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewsletterModuleDescriptor" /> class.
        /// </summary>
        public NewsletterModuleDescriptor(ICmsConfiguration cmsConfiguration)
            : base(cmsConfiguration)
        {
            newsletterJsModuleIncludeDescriptor = new NewsletterJsModuleIncludeDescriptor(this);
        }

        /// <summary>
        /// Gets the name of module.
        /// </summary>
        /// <value>
        /// The name of pages module.
        /// </value>
        public override string Name
        {
            get
            {
                return ModuleName;
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>
        /// The module description.
        /// </value>
        public override string Description
        {
            get
            {
                return "A newsletter module for Better CMS.";
            }
        }

        /// <summary>
        /// Registers module types.
        /// </summary>
        /// <param name="context">The area registration context.</param>
        /// <param name="containerBuilder">The container builder.</param>        
        public override void RegisterModuleTypes(ModuleRegistrationContext context, ContainerBuilder containerBuilder)
        {

            containerBuilder.RegisterType<DefaultSubscriberService>().AsImplementedInterfaces().InstancePerLifetimeScope();
        }

        /// <summary>
        /// Gets known client side modules in page module.
        /// </summary>        
        /// <returns>List of known client side modules in page module.</returns>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed. Suppression is OK here.")]
        public override IEnumerable<JsIncludeDescriptor> RegisterJsIncludes()
        {
            return new[] { newsletterJsModuleIncludeDescriptor };
        }

        /// <summary>
        /// Registers the site settings projections.
        /// </summary>
        /// <param name="containerBuilder">The container builder.</param>
        /// <returns>List of page action projections.</returns>
        public override IEnumerable<IPageActionProjection> RegisterSiteSettingsProjections(ContainerBuilder containerBuilder)
        {
            return new IPageActionProjection[]
                {
                    new SeparatorProjection(9999),
                    new LinkActionProjection(newsletterJsModuleIncludeDescriptor, page => "loadSiteSettingsNewsletterSubscribers")
                        {
                            Order = 9999,
                            Title = () => NewsletterGlobalization.SiteSettings_NewsletterSubscribersMenuItem,
                            CssClass = page => "bcms-sidebar-link",
                            AccessRole = RootModuleConstants.UserRoles.MultipleRoles(RootModuleConstants.UserRoles.Administration)
                        }
                };
        }

        /// <summary>
        /// Updates the data base.
        /// </summary>
        public override void UpdateDataBaseContent()
        {
            using (var pagesApi = CmsContext.CreateApiContextOf<PagesApiContext>())
            {
                const string WidgetPath = "~/Areas/bcms-newsletter/Views/Widgets/SubscribeToNewsletter.cshtml";
                var widgets = pagesApi.GetServerControlWidgets(e => e.Url == WidgetPath);
                if (widgets.Count > 0)
                {
                    return;
                }

                using (var transactionScope = new TransactionScope())
                {
                    pagesApi.CreateServerControlWidget(
                    new CreateServerControlWidgetRequest()
                        {
                            Name = "Newsletter Widget",
                            WidgetPath = WidgetPath,
                            Options =
                                new List<ContentOptionDto>()
                                    {
                                        new ContentOptionDto() { Type = OptionType.Text, Key = "Email placeholder", DefaultValue = "email..." },
                                        new ContentOptionDto() { Type = OptionType.Text, Key = "Label title", DefaultValue = "Subscribe to newsletter" },
                                        new ContentOptionDto() { Type = OptionType.Text, Key = "Submit title", DefaultValue = "Submit" },
                                        new ContentOptionDto() { Type = OptionType.Text, Key = "Submit is disabled", DefaultValue = "false" }
                                    }
                        });
                    transactionScope.Complete();
                }
            }
        }
    }
}
