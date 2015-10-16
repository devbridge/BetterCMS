using System;
using System.Collections.Generic;

using Autofac;
using Autofac.Core;

using BetterModules.Core.DataAccess;
using BetterCms.Core.DataContracts;
using BetterCms.Core.Exceptions;
using BetterCms.Core.Modules.Projections;

using BetterModules.Core.Web.Dependencies;

using NHibernate.Proxy.DynamicProxy;

namespace BetterCms.Module.Root.Projections
{
    public class PageJavaScriptProjectionFactory
    {
        private PerWebRequestContainerProvider containerProvider;

        private IRepository repository;

        public PageJavaScriptProjectionFactory(PerWebRequestContainerProvider containerProvider, IRepository repository)
        {
            this.containerProvider = containerProvider;
            this.repository = repository;
        }

        public PageJavaScriptProjection Create(IPage page, IEnumerable<IOptionValue> options)
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
                                                                 new PositionalParameter(0, page),
                                                                 new PositionalParameter(1, options)
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