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

            var openGraphMetaTitle = new OpenGraphMetaDataProjection("title", args.RenderPageData.PageData.Title);
            AddMetaDataIfNotExistsInArgs(args, openGraphMetaTitle);

            var openGraphMetaUrl = new OpenGraphMetaDataProjection("url", httpContextAccessor.MapPublicPath(args.RenderPageData.PageData.PageUrl));
            AddMetaDataIfNotExistsInArgs(args, openGraphMetaUrl);

            var pageModel = args.RenderPageData.GetPageModel();

            if (pageModel.MainImage != null)
            {
                var openGraphImage = new OpenGraphMetaDataProjection("image", pageModel.MainImage.PublicUrl);
                AddMetaDataIfNotExistsInArgs(args, openGraphImage);
            }
            else
            {
                if (pageModel.FeaturedImage != null)
                {
                    var openGraphImage = new OpenGraphMetaDataProjection("image", pageModel.FeaturedImage.PublicUrl);
                    AddMetaDataIfNotExistsInArgs(args, openGraphImage);
                }
                else if (pageModel.SecondaryImage != null)
                {
                    var openGraphImage = new OpenGraphMetaDataProjection("image", pageModel.SecondaryImage.PublicUrl);
                    AddMetaDataIfNotExistsInArgs(args, openGraphImage);
                }
            }

            if (!string.IsNullOrEmpty(pageModel.Description))
            {
                var openGraphDescription = new OpenGraphMetaDataProjection("description", pageModel.Description);
                AddMetaDataIfNotExistsInArgs(args, openGraphDescription);
            }
        }

        private void AddMetaDataIfNotExistsInArgs(PageRenderingEventArgs args, OpenGraphMetaDataProjection item)
        {
            if (!args.RenderPageData.Metadata.Contains(item))
            {
                args.RenderPageData.Metadata.Add(item);
            }
        }
    }
}