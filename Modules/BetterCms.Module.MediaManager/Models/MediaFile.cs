// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MediaFile.cs" company="Devbridge Group LLC">
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

using BetterCms.Core.Security;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.MediaManager.Models
{
    [Serializable]
    public class MediaFile : Media, IAccessSecuredObject
    {
        public const string CategorizableItemKeyForFiles = "Files";

        public virtual string OriginalFileName { get; set; }

        public virtual string OriginalFileExtension { get; set; }

        public virtual Uri FileUri { get; set; }

        public virtual string PublicUrl { get; set; }

        public virtual long Size { get; set; }

        public virtual bool IsTemporary { get; set; }

        public virtual bool? IsUploaded { get; set; }

        public virtual bool IsCanceled { get; set; }

        public virtual IList<AccessRule> AccessRules { get; set; }

        public virtual bool SaveUnsecured { get; set; }

        /// <summary>
        /// Gets or sets the rules.
        /// </summary>
        /// <value>
        /// The rules.
        /// </value>
        IList<IAccessRule> IAccessSecuredObject.AccessRules
        {
            get
            {
                if (AccessRules == null)
                {
                    return null;
                }

                return AccessRules.Cast<IAccessRule>().ToList();
            }
        }

        public virtual bool IsMovedToTrash { get; set; }

        public virtual DateTime? NextTryToMoveToTrash { get; set; }

        public virtual void AddRule(IAccessRule accessRule)
        {
            if (AccessRules == null)
            {
                AccessRules = new List<AccessRule>();
            }

            AccessRules.Add((AccessRule)accessRule);
        }

        public virtual void RemoveRule(IAccessRule accessRule)
        {
            AccessRules.Remove((AccessRule)accessRule);
        }

        public override string GetCategorizableItemKey()
        {
            return CategorizableItemKeyForFiles;
        }

        public override Media Clone()
        {
            return CopyDataTo(new MediaFile());
        }

        public override Media CopyDataTo(Media media, bool copyCollections = true)
        {
            var copy = (MediaFile)base.CopyDataTo(media, copyCollections);

            copy.OriginalFileName = OriginalFileName;
            copy.OriginalFileExtension = OriginalFileExtension;
            copy.FileUri = FileUri;
            copy.PublicUrl = PublicUrl;
            copy.Size = Size;
            copy.IsTemporary = IsTemporary;
            copy.IsUploaded = IsUploaded;
            copy.IsCanceled = IsCanceled;

            return copy;
        }
    }
}