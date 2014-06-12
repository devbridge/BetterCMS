using System;
using System.Collections.Generic;
using System.Linq;

using Autofac;
using Autofac.Core;

using BetterCms.Core.DataContracts;
using BetterCms.Core.Dependencies;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Content.Resources;

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
            return Create(pageContent, content, options, null, (pc, c, a, ch) => new PageContentProjection(pc, c, a, ch));
        }

        public PageContentProjection Create(IPageContent pageContent, IContent content, IList<IOptionValue> options, 
            IList<ChildContentProjection> childContentProjections)
        {
            return Create(pageContent, content, options, childContentProjections, (pc, c, a, ch) => new PageContentProjection(pc, c, a, ch));
        }

        public TProjection Create<TProjection>(IPageContent pageContent, IContent content, IList<IOptionValue> options,
            IList<ChildContentProjection> childContentProjections,
            Func<IPageContent, IContent, IContentAccessor, IList<ChildContentProjection>, TProjection> createProjectionDelegate)
            where TProjection : PageContentProjection
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

            /*List<ChildContentProjection> childProjections;
            if (content.Children != null)
            {
                childProjections = new List<ChildContentProjection>();
                foreach (var child in content.Children.Distinct())
                {
                    var childProjection = Create(pageContent, child.ChildContent, null, (pc, c, a, ch) => new ChildContentProjection(pc, child, a, ch));
                    childProjections.Add(childProjection);
                }
            }
            else
            {
                childProjections = null;
            }*/

            TProjection pageContentProjection = createProjectionDelegate.Invoke(pageContent, content, contentAccessor, childContentProjections);

            return pageContentProjection;
        }
    }
}