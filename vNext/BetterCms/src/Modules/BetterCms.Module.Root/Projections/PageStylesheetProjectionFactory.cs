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
    public class PageStylesheetProjectionFactory
    {
        private PerWebRequestContainerProvider containerProvider;

        private IRepository repository;

        public PageStylesheetProjectionFactory(PerWebRequestContainerProvider containerProvider, IRepository repository)
        {
            this.containerProvider = containerProvider;
            this.repository = repository;
        }

        public PageStylesheetProjection Create(IPage page, IEnumerable<IOptionValue> options)
        {
            IStylesheetAccessor jsAccessor = null;            
            Type pageType;
            if (page is IProxy)
            {
                pageType = page.GetType().BaseType;
            }
            else
            {
                pageType = page.GetType();
            }

            string key = "STYLESHEETRENDERER-" + pageType.Name.ToUpperInvariant();

            if (containerProvider.CurrentScope.IsRegisteredWithKey<IStylesheetAccessor>(key))
            {                
                jsAccessor = containerProvider.CurrentScope
                    .ResolveKeyed<IStylesheetAccessor>(key, new Parameter[]
                                                             {
                                                                 new PositionalParameter(0, page),
                                                                 new PositionalParameter(1, options)
                                                             });
            }

            if (jsAccessor == null)
            {
                throw new CmsException(string.Format("No page style sheet accessor was found for the page type {0}.", pageType.FullName));
            }

            var jsProjection = new PageStylesheetProjection(page, jsAccessor);
            return jsProjection;
        }
    }
}