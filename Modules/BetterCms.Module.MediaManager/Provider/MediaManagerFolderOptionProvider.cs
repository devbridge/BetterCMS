// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MediaManagerFolderOptionProvider.cs" company="Devbridge Group LLC">
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
using System.Linq;

using BetterModules.Core.DataAccess;
using BetterCms.Module.MediaManager.Content.Resources;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Root.Providers;

namespace BetterCms.Module.MediaManager.Provider
{
    public class MediaManagerFolderOptionProvider : ICustomOptionProvider
    {
        public const string Identifier = "media-images-folder";

        /// <summary>
        /// Converts the value to correct type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// Value, converted to correct type
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public object ConvertValueToCorrectType(string value)
        {
            Guid guid;
            if (Guid.TryParse(value, out guid))
            {
                return guid;
            }

            return null;
        }

        /// <summary>
        /// Gets the default value for type.
        /// </summary>
        /// <returns>
        /// Default value for provider type
        /// </returns>
        public object GetDefaultValueForType()
        {
            return null;
        }

        /// <summary>
        /// Gets the titles for values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <param name="repository">The repository.</param>
        /// <returns>
        /// The dictionary with value - title pairs
        /// </returns>
        public Dictionary<string, string> GetTitlesForValues(string[] values, IRepository repository)
        {
            if (values == null || values.Length == 0)
            {
                return null;
            }

            var guids = new List<Guid>(values.Length);

            foreach (var value in values)
            {
                var guid = ConvertValueToCorrectType(value);
                if (guid != null)
                {
                    guids.Add((Guid)guid);
                }
            }

            Dictionary<string, string> result;
            if (guids.Count > 0)
            {
                result = repository
                    .AsQueryable<MediaFolder>()
                    .Where(a => guids.Contains(a.Id))
                    .Select(a => new { a.Id, a.Title })
                    .ToDictionary(a => a.Id.ToString(), a => a.Title);
            }
            else
            {
                result = new Dictionary<string, string>();
            }

            if (values.Any(string.IsNullOrEmpty))
            {
                result.Add(string.Empty, MediaGlobalization.RootFolder_Title);
            }

            return result;
        }
    }
}