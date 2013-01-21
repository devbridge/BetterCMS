using System;
using System.Linq;

using Autofac;
using Autofac.Core;

using BetterCms.Core.Dependencies;
using BetterCms.Core.Exceptions;
using BetterCms.Core.Models;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Models;

using Common.Logging;

using NHibernate.Proxy.DynamicProxy;

namespace BetterCms.Module.Root.Projections
{
    public class PageContentProjectionFactory
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private PerWebRequestContainerProvider containerProvider;

        public PageContentProjectionFactory(PerWebRequestContainerProvider containerProvider)
        {
            this.containerProvider = containerProvider;
        }

        public PageContentProjection Create(PageContent pageContent)
        {
            IContentAccessor contentAccessor = null;            
            Type contentType;
            if (pageContent.Content is IProxy)
            {
                contentType = pageContent.Content.GetType().BaseType;
            }
            else
            {
                contentType = pageContent.Content.GetType();
            }

            string key = "CONTENTRENDERER-" + contentType.Name.ToUpperInvariant();

            if (containerProvider.CurrentScope.IsRegisteredWithKey<IContentAccessor>(key))
            {                
                contentAccessor = containerProvider.CurrentScope
                    .ResolveKeyed<IContentAccessor>(key, new Parameter[]
                                                             {
                                                                 new PositionalParameter(0, pageContent.Content),
                                                                 new PositionalParameter(1, pageContent.Options.Cast<IPageContentOption>().ToList())
                                                             });
            }

            if (contentAccessor == null)
            {
                Log.Error(string.Format("A content accessor was not found for the content type {0} with id={1}.", pageContent.Content.GetType().FullName, pageContent.Content.Id));

                contentAccessor = new EmptyContentAccessor(string.Format("<i style=\"color:red;\">{0}</i>", RootGlobalization.Message_FailedToRenderContent));
            }

            PageContentProjection pageContentProjection = new PageContentProjection(pageContent,  contentAccessor);
            return pageContentProjection;
        }
    }
}