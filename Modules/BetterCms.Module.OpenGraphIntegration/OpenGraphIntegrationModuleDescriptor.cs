using System;

using Autofac;

using BetterCms.Core.Dependencies;
using BetterCms.Core.Modules;
using BetterCms.Core.Web;
using BetterCms.Events;
using BetterCms.Module.OpenGraphIntegration.Projections;
using BetterCms.Module.Pages.Helpers.Extensions;


namespace BetterCms.Module.OpenGraphIntegration
{
    public class OpenGraphIntegrationModuleDescriptor : ModuleDescriptor
    {
        internal const string ModuleName = "opengraph_integration";

        internal const string ModuleId = "0528b726-7fe9-489c-84c3-6af89479a5ad";

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public override Guid Id
        {
            get
            {
                return new Guid(ModuleId);
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
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
        /// The description.
        /// </value>
        public override string Description
        {
            get
            {
                return "The Open Graph integration module for Better CMS.";
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGraphIntegrationModuleDescriptor"/> class
        /// </summary>
        /// <param name="configuration">The configuration</param>
        public OpenGraphIntegrationModuleDescriptor(ICmsConfiguration configuration)
            : base(configuration)
        {
            RootEvents.Instance.PageRendering += Events_PageRendering;
        }

        /// <summary>
        /// Add google analytics script accessor to Page.
        /// </summary>
        /// <param name="args">The args.</param>
        private void Events_PageRendering(PageRenderingEventArgs args)
        {
            IHttpContextAccessor httpContextAccessor;

            using (var container = ContextScopeProvider.CreateChildContainer())
            {
                httpContextAccessor = container.Resolve<IHttpContextAccessor>();
            }
            args.RenderPageData.Metadata.Add(new OpenGraphMetaDataProjection("title", args.RenderPageData.PageData.Title));
            args.RenderPageData.Metadata.Add(new OpenGraphMetaDataProjection("url", httpContextAccessor.MapPublicPath(args.RenderPageData.PageData.PageUrl)));
            if (args.RenderPageData.GetPageModel().MainImage != null)
            {
                args.RenderPageData.Metadata.Add(
                    new OpenGraphMetaDataProjection("image", args.RenderPageData.GetPageModel().MainImage.PublicUrl));
            }
            else
            {
                if (args.RenderPageData.GetPageModel().FeaturedImage != null)
                {
                    args.RenderPageData.Metadata.Add(
                        new OpenGraphMetaDataProjection("image", args.RenderPageData.GetPageModel().FeaturedImage.PublicUrl));
                }
                else
                {
                    if (args.RenderPageData.GetPageModel().SecondaryImage != null)
                    {
                        args.RenderPageData.Metadata.Add(
                            new OpenGraphMetaDataProjection("image", args.RenderPageData.GetPageModel().SecondaryImage.PublicUrl));
                    }

                }
            }

            if (!string.IsNullOrEmpty(args.RenderPageData.GetPageModel().Description))
            {
                args.RenderPageData.Metadata.Add(new OpenGraphMetaDataProjection("description", args.RenderPageData.GetPageModel().Description));
            }
        }
    }
}