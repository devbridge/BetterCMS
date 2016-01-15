// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageStylesheetProjectionFactory.cs" company="Devbridge Group LLC">
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