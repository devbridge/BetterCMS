using System;
using System.Collections.Generic;

using Autofac;
using Autofac.Core;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Dependencies;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.ViewModels.Option;

using Common.Logging;

using NHibernate.Proxy.DynamicProxy;

namespace BetterCms.Module.Root.Projections
{
    public class PageContentProjectionFactory
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly PerWebRequestContainerProvider containerProvider;

        public PageContentProjectionFactory(PerWebRequestContainerProvider containerProvider)
        {
            this.containerProvider = containerProvider;
        }

        public PageContentProjection Create(IPageContent pageContent, IContent content, IList<IOptionValue> options)
        {
            IContentAccessor contentAccessor = null;            
            Type contentType;

            if (content is IProxy)
            {
                contentType = content.GetType().BaseType;
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

            if (contentAccessor == null)
            {
                Log.Error(string.Format("A content accessor was not found for the content type {0} with id={1}.", content.GetType().FullName, content.Id));

                contentAccessor = new EmptyContentAccessor(string.Format("<i style=\"color:red;\">{0}</i>", RootGlobalization.Message_FailedToRenderContent));
            }

            PageContentProjection pageContentProjection = new PageContentProjection(pageContent, content, contentAccessor);

            return pageContentProjection;
        }
    }
}