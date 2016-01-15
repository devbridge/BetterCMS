// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageContentProjectionFactory.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;

using Autofac;
using Autofac.Core;

using BetterModules.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts;
using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Content.Resources;

using Common.Logging;

using BetterModules.Core.Web.Dependencies;

using NHibernate.Proxy.DynamicProxy;

namespace BetterCms.Module.Root.Projections
{
    public class PageContentProjectionFactory
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly PerWebRequestContainerProvider containerProvider;
        private readonly IUnitOfWork unitOfWork;

        public PageContentProjectionFactory(PerWebRequestContainerProvider containerProvider, IUnitOfWork unitOfWork)
        {
            this.containerProvider = containerProvider;
            this.unitOfWork = unitOfWork;
        }

        public virtual TProjection Create<TProjection>(IPageContent pageContent, IContent content, IList<IOptionValue> options,
            IEnumerable<ChildContentProjection> childContentProjections, IEnumerable<PageContentProjection> childRegionContentProjections,
            Func<IPageContent, IContent, IContentAccessor, IEnumerable<ChildContentProjection>, IEnumerable<PageContentProjection>, TProjection> createProjectionDelegate)
            where TProjection : PageContentProjection
        {
            IContentAccessor contentAccessor = GetAccessorForType(content, options);

            if (contentAccessor == null)
            {
                Log.Error(string.Format("A content accessor was not found for the content type {0} with id={1}.", content.GetType().FullName, content.Id));

                contentAccessor = new EmptyContentAccessor(string.Format("<i style=\"color:red;\">{0}</i>", RootGlobalization.Message_FailedToRenderContent));
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