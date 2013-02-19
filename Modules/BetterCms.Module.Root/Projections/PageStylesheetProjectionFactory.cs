using System;

using Autofac;
using Autofac.Core;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Dependencies;
using BetterCms.Core.Exceptions;
using BetterCms.Core.Models;
using BetterCms.Core.Modules.Projections;

using NHibernate.Proxy.DynamicProxy;

namespace BetterCms.Module.Root.Projections
{
    public class PageStylesheetProjectionFactory
    {
        private PerWebRequestContainerProvider containerProvider;

        public PageStylesheetProjectionFactory(PerWebRequestContainerProvider containerProvider)
        {
            this.containerProvider = containerProvider;
        }

        public PageStylesheetProjection Create(IPage page)
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
                                                                 new PositionalParameter(0, page)
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