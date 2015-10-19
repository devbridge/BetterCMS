using System;
using System.Collections.Generic;

using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Content.Resources;
using Microsoft.Framework.Logging;
using NHibernate.Proxy.DynamicProxy;

namespace BetterCms.Module.Root.Projections
{
    public class PageContentProjectionFactory
    {
        private readonly ILogger logger;
        private readonly IUnitOfWork unitOfWork;

        public PageContentProjectionFactory(IUnitOfWork unitOfWork, ILoggerFactory loggerFactory)
        {
            this.unitOfWork = unitOfWork;
            logger = loggerFactory.CreateLogger<PageContentProjectionFactory>();
        }

        public virtual TProjection Create<TProjection>(IPageContent pageContent, IContent content, IList<IOptionValue> options,
            IEnumerable<ChildContentProjection> childContentProjections, IEnumerable<PageContentProjection> childRegionContentProjections,
            Func<IPageContent, IContent, IContentAccessor, IEnumerable<ChildContentProjection>, IEnumerable<PageContentProjection>, TProjection> createProjectionDelegate)
            where TProjection : PageContentProjection
        {
            IContentAccessor contentAccessor = GetAccessorForType(content, options);

            if (contentAccessor == null)
            {
                logger.LogError(
                    $"A content accessor was not found for the content type {content.GetType().FullName} with id={content.Id}.");

                contentAccessor = new EmptyContentAccessor(
                    $"<i style=\"color:red;\">{RootGlobalization.Message_FailedToRenderContent}</i>");
            }

            TProjection pageContentProjection = createProjectionDelegate.Invoke(pageContent, content, contentAccessor, childContentProjections, childRegionContentProjections);

            return pageContentProjection;
        }

        public virtual IContentAccessor GetAccessorForType(IContent content, IList<IOptionValue> options = null)
        {
            IContentAccessor contentAccessor = null;
            Type contentType;

            if (content is IProxy)
            {
                content = (IContent)unitOfWork.Session.GetSessionImplementation().PersistenceContext.Unproxy(content);
                contentType = content.GetType();
            }
            else
            {
                contentType = content.GetType();
            }

            string key = "CONTENTRENDERER-" + contentType.Name.ToUpperInvariant();

            if (containerProvider.CurrentScope.IsRegisteredWithKey<IContentAccessor>(key))
            {
                contentAccessor = containerProvider.CurrentScope
                    .ResolveKeyed<IContentAccessor>(key, new Parameter[]
                                                             {
                                                                 new PositionalParameter(0, content),
                                                                 new PositionalParameter(1, options)
                                                             });
            }

            return contentAccessor;
        }
    }
}