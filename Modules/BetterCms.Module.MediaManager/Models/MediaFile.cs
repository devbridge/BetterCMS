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