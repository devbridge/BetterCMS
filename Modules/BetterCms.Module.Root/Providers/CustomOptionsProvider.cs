// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomOptionsProvider.cs" company="Devbridge Group LLC">
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
using System.Collections.Generic;
using System.Linq;

using Autofac;

using BetterModules.Core.DataAccess;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.ViewModels.Option;

namespace BetterCms.Module.Root.Providers
{
    public static class CustomOptionsProvider
    {
        /// <summary>
        /// The list of custom options providers
        /// </summary>
        private static Dictionary<string, ICustomOptionProvider> providers = new Dictionary<string, ICustomOptionProvider>();

        /// <summary>
        /// Registers the provider.
        /// </summary>
        public static void RegisterProvider(string identifier, ICustomOptionProvider provider)
        {
            if (!providers.ContainsKey(identifier))
            {
                providers.Add(identifier, provider);
            }
        }

        /// <summary>
        /// Gets the provider.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns></returns>
        public static ICustomOptionProvider GetProvider(string identifier)
        {
            if (string.IsNullOrEmpty(identifier))
            {
                return null;
            }

            return providers[identifier];
        }
    }
}