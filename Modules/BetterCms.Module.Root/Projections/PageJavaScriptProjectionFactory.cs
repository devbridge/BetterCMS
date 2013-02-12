using System;

using Autofac;
using Autofac.Core;

using BetterCms.Api.Interfaces.Models;
using BetterCms.Core.Dependencies;
using BetterCms.Core.Exceptions;
using BetterCms.Core.Models;
using BetterCms.Core.Modules.Projections;

using NHibernate.Proxy.DynamicProxy;

namespace BetterCms.Module.Root.Projections
{
    public class PageJavaScriptProjectionFactory
    {
        private PerWebRequestContainerProvider containerProvider;

        public PageJavaScriptProjectionFactory(PerWebRequestContainerProvider containerProvider)
        {
            this.containerProvider = containerProvider;
        }

        public PageJavaScriptProjection Create(IPage page)
        {
            IJavaScriptAccessor jsAccessor = null;            
            Type pageType;
            if (page is IProxy)
            {
                pageType = page.GetType().BaseType;
            }
            else
            {
                pageType = page.GetType();
            }

            string key = "JAVASCRIPTRENDERER-" + pageType.Name.ToUpperInvariant();

            if (containerProvider.CurrentScope.IsRegisteredWithKey<IJavaScriptAccessor>(key))
            {                
                jsAccessor = containerProvider.CurrentScope
                    .ResolveKeyed<IJavaScriptAccessor>(key, new Parameter[]
                                                             {
                                                                 new PositionalParameter(0, page)
                                                             });
            }

            if (jsAccessor == null)
            {
                throw new CmsException(string.Format("No page javascript accessor was found for the page type {0}.", pageType.FullName));
            }

            var jsProjection = new PageJavaScriptProjection(page, jsAccessor);
            return jsProjection;
        }
    }
}