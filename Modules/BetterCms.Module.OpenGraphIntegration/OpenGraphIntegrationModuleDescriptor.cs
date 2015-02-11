using Autofac;

using BetterCms.Core.Modules;

using BetterCms.Events;

using BetterCms.Module.OpenGraphIntegration.Projections;
using BetterCms.Module.Pages.Helpers.Extensions;

using Devbridge.Platform.Core.Dependencies;
using Devbridge.Platform.Core.Web.Web;

namespace BetterCms.Module.OpenGraphIntegration
{
    public class OpenGraphIntegrationModuleDescriptor : CmsModuleDescriptor
    {
        internal const string ModuleName = "opengraph_integration";

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

            var pageModel = args.RenderPageData.GetPageModel();

            if (pageModel.MainImage != null)
            {
                args.RenderPageData.Metadata.Add(
                    new OpenGraphMetaDataProjection("image", pageModel.MainImage.PublicUrl));
            }
            else
            {
                if (pageModel.FeaturedImage != null)
                {
                    args.RenderPageData.Metadata.Add(
                        new OpenGraphMetaDataProjection("image", pageModel.FeaturedImage.PublicUrl));
                }
                else
                {
                    if (pageModel.SecondaryImage != null)
                    {
                        args.RenderPageData.Metadata.Add(
                            new OpenGraphMetaDataProjection("image", pageModel.SecondaryImage.PublicUrl));
                    }

                }
            }

            if (!string.IsNullOrEmpty(pageModel.Description))
            {
                args.RenderPageData.Metadata.Add(new OpenGraphMetaDataProjection("description", pageModel.Description));
            }
        }
    }
}