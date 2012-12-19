using System;
using System.Linq;

using Autofac;
using Autofac.Core;

using BetterCms.Core.Dependencies;
using BetterCms.Core.Exceptions;
using BetterCms.Core.Models;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Models;

using NHibernate.Proxy.DynamicProxy;

namespace BetterCms.Module.Root.Projections
{
    public class PageContentProjectionFactory
    {        
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
                                                                 new PositionalParameter(1, pageContent.PageContentOptions.Cast<IPageContentOption>().ToList())
                                                             });
            }

            if (contentAccessor == null)
            {
                throw new CmsException(string.Format("No content accessor found for the content type {0}.", pageContent.Content.GetType().FullName));
            }

            PageContentProjection pageContentProjection = new PageContentProjection(pageContent,  contentAccessor);
            return pageContentProjection;
        }
    }
}