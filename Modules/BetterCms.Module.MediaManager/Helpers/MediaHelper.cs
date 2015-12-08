// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MediaHelper.cs" company="Devbridge Group LLC">
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
using System.IO;
using System.Linq;
using System.Web;

using BetterCms.Core.DataContracts;
using BetterCms.Module.MediaManager.Models;

using BetterModules.Core.DataAccess;

using NHibernate.Linq;

namespace BetterCms.Module.MediaManager.Helpers
{
    public class MediaHelper
    {
        public static void SetCategories(IRepository repository, Media sourceMedia, Media destinationMedia)
        {
            var destination = destinationMedia as ICategorized;
            var source = sourceMedia as ICategorized;

            if (destination == null || source == null)
            {
                return;
            }

            if (destination.Categories != null)
            {
                var categoriesToRemove = destination.Categories.ToList();
                categoriesToRemove.ForEach(repository.Delete);
            }

            if (source.Categories == null)
            {
                return;
            }

            source.Categories.ForEach(destination.AddCategory);
            if (destination.Categories != null)
            {
                destination.Categories.ForEach(e => e.SetEntity(destinationMedia));
            }
        }

        public static void SetTags(IRepository repository, Media sourceMedia, Media destinationMedia)
        {
            var destination = destinationMedia;
            var source = sourceMedia;

            if (destination == null || source == null)
            {
                return;
            }

            if (destination.MediaTags != null)
            {
                var tagsToRemove = destination.MediaTags.ToList();
                tagsToRemove.ForEach(repository.Delete);
            }

            if (source.MediaTags == null)
            {
                return;
            }

            source.MediaTags.ForEach(destination.AddTag);
            if (destination.MediaTags != null)
            {
                foreach (var mediaTag in destination.MediaTags)
                {
                    mediaTag.Media = destinationMedia;
                }
            }
        }

        public static void SetCollections(IRepository repository, Media sourceMedia, Media destinationMedia)
        {
            SetCategories(repository, sourceMedia, destinationMedia);
            SetTags(repository, sourceMedia, destinationMedia);
        }

        public static string RemoveInvalidPathSymbols(string fileName)
        {
            var invalidFileNameChars = Path.GetInvalidFileNameChars().ToList();
            invalidFileNameChars.AddRange(new[] { '+', ' ' , '&' });
            return HttpUtility.UrlEncode(invalidFileNameChars.Aggregate(fileName, (current, invalidFileNameChar) => current.Replace(invalidFileNameChar, '_')));
        }
    }
}