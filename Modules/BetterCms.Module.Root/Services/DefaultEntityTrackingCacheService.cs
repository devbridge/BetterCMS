// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultEntityTrackingCacheService.cs" company="Devbridge Group LLC">
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
using System.Web;

using BetterCms.Core.Web;

using BetterModules.Core.Web.Web;

namespace BetterCms.Module.Root.Services
{
    public class DefaultEntityTrackingCacheService : IEntityTrackingCacheService
    {
        private readonly IHttpContextAccessor contextAccessor;

        public DefaultEntityTrackingCacheService(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        public bool GetEntity(System.Type type, System.Guid id, out object entity)
        {
            entity = null;

            var context = GetCurrentContext();
            var key = CreateKey(type, id);
            if (context != null)
            {
                entity = context.Items[key];
                return entity != null;
            }

            return false;
        }

        public void AddEntity(System.Type type, System.Guid id, object entity)
        {
            if (entity == null)
            {
                return;
            }

            var context = GetCurrentContext();
            var key = CreateKey(type, id);

            if (context != null)
            {
                if (context.Items.Contains(key))
                {
                    context.Items.Remove(key);
                }

                context.Items.Add(key, entity);
            }
        }

        private string CreateKey(System.Type type, System.Guid id)
        {
            return string.Format("{0}_{1}", type, id);
        }

        private HttpContextBase GetCurrentContext()
        {
            if (contextAccessor != null)
            {
                return contextAccessor.GetCurrent();
            }

            return null;
        }
    }
}